using System;
using Meta.Net.Sync.Types;
using Meta.Net.Interfaces;

namespace Meta.Net.Sync
{
    /// <summary>
    /// Class derived from SyncAction for database driven synchronization actions
    /// to be used in DataSyncActionCollection objects and used by SqlServerSyncManager objects
    /// as steps towards completing database synchronization.
    /// </summary>
    public class DataSyncAction : SyncAction
    {
		public IMetaObject DataObject { get; private set; }

        /// <summary>
        /// SQL Query to be executed in SyncManager type objects.
        /// </summary>
        public Func<string> ScriptAction { get; private set; }
        public string Script
        {
            get
            {
                return ScriptAction();
            }
        }
        /// <summary>
        /// Enumerated Type of data sync operations such as CreateUserTable.
        /// </summary>
        public DataSyncOperationType Type { get; private set; }

		/// <summary>
        /// Constructor that initializes all interface members and derived members.
        /// </summary>
        /// <param name="dataObject">DataObject to sync.</param>
        /// <param name="identifier">Identifier of the sync action.</param>
        /// <param name="description">Description of the sync action.</param>
        /// <param name="process">Whether or not to process the sync action.</param>
        /// <param name="type">Enumerated Type of data sync operations such as CreateUserTable.</param>
        /// <param name="script">SQL Query to be executed in SyncManager type objects.</param>
        public DataSyncAction(IMetaObject dataObject, string identifier, string description, DataSyncOperationType type, Func<string> script, bool process)
            : base(identifier, description, process)
        {
            DataObject = dataObject;
            Type = type;
            ScriptAction = script;
        }

        /// <summary>
        /// Constructor that initializes all interface members and derived members, but
        /// Process defaults to true;
        /// </summary>
        /// <param name="dataObject">DataObject to sync.</param>
        /// <param name="identifier">Identifier of the sync action.</param>
        /// <param name="description">Description of the sync action.</param>
        /// <param name="type">Enumerated Type of data sync operations such as CreateUserTable.</param>
        /// <param name="script">SQL Query to be executed in SyncManager type objects.</param>
        public DataSyncAction(IMetaObject dataObject, string identifier, string description, DataSyncOperationType type, Func<string> script)
            : base(identifier, description)
        {
            DataObject = dataObject;
            Type = type;
            ScriptAction = script;
        } 
    }
}