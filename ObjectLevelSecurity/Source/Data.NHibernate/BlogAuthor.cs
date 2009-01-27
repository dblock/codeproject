using System;
namespace Vestris.Data.NHibernate
{

    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'BlogAuthor' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'BlogAuthor' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class BlogAuthor: IDataObject
    {
#region " Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 _Id;
        private Account _Account;
        private Blog _Blog;

        //Public properties
        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive identity property.
        ///</summary>
        ///<remarks>
        ///This property is an identity property.
        ///The identity index for this property is '0'.
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field '_Id' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'BlogAuthor_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Int32 Id
        {
            get
            {
                return _Id;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Account'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Account.BlogAuthors'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field '_Account' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual Account Account
        {
            get
            {
                return _Account;
            }
            set
            {
                if (_Account != null)
                {
                    throw new InvalidOperationException();
                }

                _Account = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Blog'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Blog.BlogAuthors'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field '_Blog' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Blog_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual Blog Blog
        {
            get
            {
                return _Blog;
            }
            set
            {
                _Blog = value;
            }
        }

#endregion //Generated Code Region

        //Add your synchronized custom code here:
#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

        //Add your unsynchronized custom code here:
#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
}
