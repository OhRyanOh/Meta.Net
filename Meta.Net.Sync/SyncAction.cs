﻿using Meta.Net.Sync.Interfaces;

namespace Meta.Net.Sync
{
    public class SyncAction : ISyncAction
    {
        /// <summary>
        /// Identifier of the sync action.
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Description of the sync action.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Whether or not to process the sync action.
        /// </summary>
        public bool Process { get; set; }

        /// <summary>
        /// Constructor that initializes all members.
        /// </summary>
        /// <param name="identifier">Identifier of the sync action.</param>
        /// <param name="description">Description of the sync action.</param>
        /// <param name="process">Whether or not to process the sync action.</param>
        public SyncAction(string identifier, string description, bool process)
        {
            Identifier = identifier;
            Description = description;
            Process = process;
        }

        /// <summary>
        /// Constructor that defaults Process to true.
        /// </summary>
        /// <param name="identifier">Identifier of the sync action.</param>
        /// <param name="description">Description of the sync action.</param>
        public SyncAction(string identifier, string description)
        {
            Identifier = identifier;
            Description = description;
            Process = true;
        }
    }
}
