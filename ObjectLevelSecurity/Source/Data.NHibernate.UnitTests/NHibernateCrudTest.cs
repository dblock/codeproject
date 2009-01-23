using System;
using NUnit.Framework;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;

namespace Vestris.Data.NHibernate.UnitTests
{
    public abstract class NHibernateCrudTest : NHibernateTest
    {
        private List<IDataObject> _dependentObjects = new List<IDataObject>();
        protected abstract IDataObject IDataObjectInstance { get; }

        public NHibernateCrudTest()
        {

        }

        public void AddDependentObject(NHibernateCrudTest test)
        {
            _dependentObjects.AddRange(test._dependentObjects);
            _dependentObjects.Add(test.IDataObjectInstance);
        }

        public void AddDependentObject(IDataObject instance)
        {
            _dependentObjects.Add(instance);
        }

        public int Create()
        {
            Console.WriteLine("Create");
            Console.WriteLine(" Creating " + IDataObjectInstance.ToString());
            SaveDependentObjects();
            int id = SaveObject(IDataObjectInstance);
            Session.Flush();
            return id;
        }

        public void Delete()
        {
            DeleteObject(IDataObjectInstance);
            DeleteDependentObjects();
            Session.Flush();
        }

        [Test]
        public void CreateAndDelete()
        {
            Create();
            Delete();
        }

        [Test]
        public void RetrieveAndUpdate()
        {
            Console.WriteLine("RetrieveAndUpdate");
            Console.WriteLine(" Creating " + IDataObjectInstance.ToString());
            SaveDependentObjects();
            SaveObject(IDataObjectInstance);
            Session.Flush();
            Console.WriteLine(" Retrieving " + IDataObjectInstance.ToString());
            object o = Session.Get(IDataObjectInstance.GetType(), IDataObjectInstance.Id);
            Console.WriteLine(" Retrieved " + o.ToString());
            Session.Update(o, IDataObjectInstance.Id);
            Session.Flush();
            DeleteObject(IDataObjectInstance);
            DeleteDependentObjects();
            Session.Flush();
        }

        protected virtual void DeleteDependentObjects()
        {
            for (int i = _dependentObjects.Count - 1; i >= 0; i--)
            {
                DeleteObject(_dependentObjects[i]);
            }
        }

        protected virtual void SaveDependentObjects()
        {
            foreach (IDataObject instance in _dependentObjects)
            {
                SaveObject(instance);
            }
        }

        private void DeleteObject(IDataObject instance)
        {
            Console.WriteLine(string.Format("  Deleting {0}: {1}", instance.ToString(),
                instance.Id));

            Session.Delete(instance);
        }

        private int SaveObject(IDataObject instance)
        {
            Console.Write(string.Format("  Creating {0}", instance.ToString()));
            Session.Save(instance);
            Console.WriteLine(string.Format(": {0}", instance.Id));
            return instance.Id;
        }
    }

    public abstract class NHibernateCrudTest<IDataObjectType> : NHibernateCrudTest
        where IDataObjectType : IDataObject, new()
    {
        protected IDataObjectType _instance = new IDataObjectType();
        protected override IDataObject IDataObjectInstance { get { return _instance; } }
        public IDataObjectType Instance { get { return _instance; } }

        public NHibernateCrudTest()
        {

        }
    }
}
