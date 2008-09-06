using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace WixDoxyFilter
{
    class Program
    {
        static void Usage()
        {
            Console.WriteLine("usage: WixDoxyFilter [filename]");
        }

        static int Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Usage();
                    throw new ArgumentException("filename");
                }

                WixPreprocessor preprocessor = new WixPreprocessor(args[0]);
                XmlDocument xml = new XmlDocument();
                preprocessor.Preprocess();

                //Console.WriteLine(preprocessor.Result);
                xml.LoadXml(preprocessor.Result);

                XmlNamespaceManager nsm = new XmlNamespaceManager(xml.NameTable);
                nsm.AddNamespace("wix", "http://schemas.microsoft.com/wix/2006/wi");

                TransformProducts(xml, nsm);
                TransformModules(xml, nsm);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        static string ExpandVariable(string id)
        {
            // todo: expand variables
            return id;
        }

        static string ExpandId(string id)
        {
            if (id == "*") return Guid.NewGuid().ToString();
            return ExpandVariable(id);
        }

        static string MakeId(string id)
        {
            string result = "";
            for (int i = 0; i < id.Length; i++)
            {
                if (!Char.IsLetterOrDigit(id[i]))
                {
                    result += '_';
                }
                else
                {
                    result += id[i];
                }
            }

            return result.ToLower();
        }

        static void TransformPackages(XmlNode product, XmlNamespaceManager nsm)
        {
            XmlNodeList packages = product.SelectNodes("//wix:Package", nsm);
            foreach (XmlNode package in packages)
            {
                XmlAttribute description = package.Attributes["Description"];
                if (description != null) Console.WriteLine("{0}", description.Value);
            }
        }

        static string GetProductId(XmlNode product)
        {
            XmlAttribute name = product.Attributes["Name"];
            string s_name = name != null ? name.Value : "Unknown";
            return MakeId(s_name);            
        }

        static void TransformComponents(XmlNode product, string product_id, XmlNamespaceManager nsm)
        {
            string components_id = string.Format("{0}_components", product_id);
            Console.WriteLine(@"\section {0} Components", components_id);
            XmlNodeList components = product.SelectNodes("//wix:Component", nsm);
            foreach (XmlNode component in components)
            {
                XmlAttribute id = component.Attributes["Id"];
                XmlAttribute guid = component.Attributes["Guid"];
                string s_id = ExpandId(id != null ? id.Value : Guid.NewGuid().ToString());
                string s_guid = guid != null ? guid.Value : string.Empty;
                string component_id = string.Format("{0}_{1}", components_id, MakeId(s_id));
                Console.WriteLine(@"\subsection {0} {1}", component_id, s_id);
                TransformComment(component);
                Console.WriteLine();
                Console.WriteLine("Component guid: {0}", s_guid);
                Console.WriteLine();
            }
        }

        static void TransformMerges(XmlNode product, string product_id, XmlNamespaceManager nsm)
        {
            string merges_id = string.Format("{0}_merges", product_id);
            Console.WriteLine(@"\section {0} Merge Modules", merges_id);
            XmlNodeList merges = product.SelectNodes("//wix:Merge", nsm);
            foreach (XmlNode merge in merges)
            {
                XmlAttribute id = merge.Attributes["Id"];
                XmlAttribute sourcefile = merge.Attributes["SourceFile"];
                string s_id = ExpandId(id != null ? id.Value : Guid.NewGuid().ToString());
                string s_sourcefile = sourcefile != null ? Path.GetFullPath(sourcefile.Value) : string.Empty;
                string merge_id = string.Format("{0}_{1}", merges_id, MakeId(s_id));
                Console.WriteLine(@"\subsection {0} {1}", merge_id, s_id);
                TransformComment(merge);
                Console.WriteLine();
                Console.WriteLine("Source: {0}", s_sourcefile.Replace(@"\", @"\\"));
                Console.WriteLine();
            }
        }

        static void TransformProductFeatures(XmlNode product, string product_id, XmlNamespaceManager nsm)
        {
            TransformProductFeatures(product, null, product_id, nsm);
        }

        static void TransformProductFeatures(XmlNode product, XmlNode parent, string product_id, XmlNamespaceManager nsm)
        {
            XmlAttribute name = product.Attributes["Name"];
            string s_name = name != null ? name.Value : "Unknown";

            string features_id = string.Format("{0}_features", product_id);
            
            if (parent == null)
            {
                Console.WriteLine(@"\section {0} Features", features_id);
            }

            XmlNodeList features = (parent != null ? parent.SelectNodes("wix:Feature", nsm) : product.SelectNodes("wix:Feature", nsm));
            foreach (XmlNode feature in features)
            {
                XmlAttribute id = feature.Attributes["Id"];
                XmlAttribute title = feature.Attributes["Title"];
                string s_id = ExpandId(id != null ? id.Value : Guid.NewGuid().ToString());
                string s_title = title != null ? title.Value : string.Empty;
                string feature_id = string.Format("{0}_{1}", features_id, MakeId(s_id));
                Console.WriteLine(@"\subsection {0} {1}", feature_id, s_title);
                TransformComment(feature);
                TransformFeatureComponents(feature, product_id, nsm);
                TransformFeatureMerges(feature, product_id, nsm);
                TransformProductFeatures(product, feature, product_id, nsm);
            }
        }

        static void TransformFeatureComponents(XmlNode feature, string product_id, XmlNamespaceManager nsm)
        {
            XmlNodeList componentrefs = feature.SelectNodes("wix:ComponentRef", nsm);
            foreach (XmlNode componentref in componentrefs)
            {
                XmlAttribute id = componentref.Attributes["Id"];
                string s_id = id != null ? id.Value : "Unknown";
                string components_id = string.Format("{0}_components", product_id);
                string component_id = string.Format("{0}_{1}", components_id, MakeId(s_id));
                Console.WriteLine(string.Format(@"\li \ref {0}", component_id));
            }
        }

        static void TransformFeatureMerges(XmlNode feature, string product_id, XmlNamespaceManager nsm)
        {
            XmlNodeList mergerefs = feature.SelectNodes("wix:MergeRef", nsm);
            foreach (XmlNode mergeref in mergerefs)
            {
                XmlAttribute id = mergeref.Attributes["Id"];
                string s_id = id != null ? id.Value : "Unknown";
                string merges_id = string.Format("{0}_merges", product_id);
                string merge_id = string.Format("{0}_{1}", merges_id, MakeId(s_id));
                Console.WriteLine(string.Format(@"\li \ref {0}", merge_id));
            }
        }

        static void TransformProducts(XmlDocument xml, XmlNamespaceManager nsm)
        {
            XmlNodeList products = xml.SelectNodes("//wix:Product", nsm);
            foreach (XmlNode product in products)
            {
                // XmlAttribute id = product.Attributes["Id"];
                XmlAttribute name = product.Attributes["Name"];
                // string s_id = ExpandId(id != null ? id.Value : Guid.NewGuid().ToString());
                string s_name = name != null ? name.Value : "Unknown";
                string product_id = string.Format("{0}_wxs", MakeId(s_name));
                Console.WriteLine(string.Format(@"/*! \page {0} {1}", product_id, s_name));
                TransformPackages(product, nsm);
                TransformComment(product);
                TransformConditions(product, product_id, nsm);
                TransformProductFeatures(product, product_id, nsm);
                TransformComponents(product, product_id, nsm);
                TransformMerges(product, product_id, nsm);
                Console.WriteLine(@"*/");
            }
        }

        static void TransformModules(XmlDocument xml, XmlNamespaceManager nsm)
        {
            XmlNodeList modules = xml.SelectNodes("//wix:Module", nsm);
            foreach (XmlNode module in modules)
            {
                XmlAttribute id = module.Attributes["Id"];
                string s_id = id != null ? id.Value : "Unknown";
                string module_id = string.Format("{0}_wxs", MakeId(s_id));
                Console.WriteLine(string.Format(@"/*! \page {0} {1}", module_id, s_id));
                TransformPackages(module, nsm);
                TransformComment(module);
                TransformComponents(module, module_id, nsm);
                Console.WriteLine(@"*/");
            }
        }

        static void TransformComment(XmlNode node)
        {
            XmlNode comment = node.PreviousSibling;
            if (comment != null && comment.NodeType == XmlNodeType.Comment)
            {
                Console.WriteLine(comment.InnerText);
            }
        }

        static void TransformConditions(XmlNode product, string product_id, XmlNamespaceManager nsm)
        {
            string conditions_id = string.Format("{0}_conditions", product_id);
            Console.WriteLine(@"\section {0} Prerequisites and Conditions", conditions_id);
            XmlNodeList conditions = product.SelectNodes("wix:Condition", nsm);
            foreach (XmlNode condition in conditions)
            {
                XmlAttribute message = condition.Attributes["Message"];
                string s_message = (message != null ? message.Value : string.Empty);
                string data = condition.InnerText.Trim();
                string condition_id = string.Format("{0}_{1}", conditions_id, MakeId(data));
                Console.WriteLine(@"\subsection {0} {1}", condition_id, data);
                TransformComment(condition);
                if (!string.IsNullOrEmpty(s_message))
                {
                    Console.WriteLine();
                    Console.WriteLine("Message: {0}", s_message);
                    Console.WriteLine();
                }
            }
        }
    }
}
