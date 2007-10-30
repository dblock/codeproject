using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration.Provider;
using System.Collections.Specialized;

namespace DBlock
{
    public class SiteMapDataProvider : StaticSiteMapProvider
    {
        private SiteMapNode mRootNode = null;

        public SiteMapDataProvider()
        {

        }

        // create the root node
        public override void Initialize(string name, NameValueCollection attributes)
        {
            base.Initialize(name, attributes);
            mRootNode = new SiteMapNode(this, "Home", "Default.aspx", "Home");
            AddNode(mRootNode);
        }

        public override SiteMapNode BuildSiteMap()
        {
            return mRootNode;
        }

        public override SiteMapNode RootNode
        {
            get
            {
                return mRootNode;
            }
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return RootNode;
        }

        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            return base.FindSiteMapNode(rawUrl);
        }

        // stack a node under the root
        public SiteMapNode Stack(string title, string uri)
        {
            return Stack(title, uri, mRootNode);
        }

        // stack a node under any other node
        public SiteMapNode Stack(string title, string uri, SiteMapNode parentnode)
        {
            lock (this)
            {
                SiteMapNode node = base.FindSiteMapNodeFromKey(uri);

                if (node == null)
                {
                    node = new SiteMapNode(this, uri, uri, title);
                    node.ParentNode = ((parentnode == null) ? mRootNode : parentnode);
                    AddNode(node);
                }
                else if (node.Title != title)
                {
                    node.Title = title;
                }

                return node;
            }
        }

        public void Stack(List<KeyValuePair<string, Uri>> nodes)
        {
            SiteMapNode parent = RootNode;
            foreach (KeyValuePair<string, Uri> node in nodes)
            {
                parent = Stack(node.Key, node.Value.PathAndQuery, parent);
            }
        }
    }
}