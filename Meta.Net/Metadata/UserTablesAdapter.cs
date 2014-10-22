﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Meta.Net.Interfaces;
using Meta.Net.Objects;

namespace Meta.Net.Metadata
{
    public static class UserTablesAdapter
    {
        public static void Read(Catalog catalog, IDataReader reader, Dictionary<string, UserTable> userTables)
        {
            var schemaNameOrdinal = reader.GetOrdinal("SchemaName");
            var objectNameOrdinal = reader.GetOrdinal("ObjectName");
            var fileStreamFileGroupOrdinal = reader.GetOrdinal("FileStreamFileGroup");
            var lobFileGroupOrdinal = reader.GetOrdinal("LobFileGroup");
            var hasTextNTextOrImageColumnsOrdinal = reader.GetOrdinal("HasTextNTextOrImageColumns");
            var usesAnsiNullsOrdinal = reader.GetOrdinal("UsesAnsiNulls");
            var textInRowLimitOrdinal = reader.GetOrdinal("TextInRowLimit");

            while (reader.Read())
            {
                var schemaName = Convert.ToString(reader[schemaNameOrdinal]);
                var objectName = Convert.ToString(reader[objectNameOrdinal]);
                var fileStreamFileGroup = Convert.ToString(reader[fileStreamFileGroupOrdinal]);
                var lobFileGroup = Convert.ToString(reader[lobFileGroupOrdinal]);
                var hasTextNTextOrImageColumns = Convert.ToBoolean(reader[hasTextNTextOrImageColumnsOrdinal]);
                var usesAnsiNulls = Convert.ToBoolean(reader[usesAnsiNullsOrdinal]);
                var textInRowLimit = Convert.ToInt32(reader[textInRowLimitOrdinal]);

                var schema = catalog.Schemas[schemaName];
                if (schema == null)
                    continue;

                var userTable = new UserTable
                {
                    Schema = schema,
                    ObjectName = objectName,
                    FileStreamFileGroup = fileStreamFileGroup,
                    LobFileGroup = lobFileGroup,
                    HasTextNTextOrImageColumns = hasTextNTextOrImageColumns,
                    UsesAnsiNulls = usesAnsiNulls,
                    TextInRowLimit = textInRowLimit
                };

                schema.UserTables.Add(userTable);
                userTables.Add(userTable.Namespace, userTable);
            }
        }

        public static Dictionary<string, UserTable> Get(Catalog catalog, DbConnection connection, IMetadataScriptFactory metadataScriptFactory)
        {
            var userTables = new Dictionary<string, UserTable>(StringComparer.OrdinalIgnoreCase);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = metadataScriptFactory.UserTables(catalog.ObjectName);
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        return userTables;
                    }

                    Read(catalog, reader, userTables);

                    reader.Close();
                }
            }

            return userTables;
        }

        public static void Build(Catalog catalog, DbConnection connection, IMetadataScriptFactory metadataScriptFactory)
        {
            var userTables = Get(catalog, connection, metadataScriptFactory);
            if (userTables.Count == 0)
                return;

            UserTableColumnsAdapter.Get(catalog, userTables, connection, metadataScriptFactory);
            var primaryKeyColumns = PrimaryKeysAdapter.Get(catalog, userTables, connection, metadataScriptFactory);
            var uniqueConstraintColumns = UniqueConstraintsAdapter.Get(catalog, userTables, connection, metadataScriptFactory);
            ForeignKeysAdapter.Get(catalog, userTables, primaryKeyColumns, uniqueConstraintColumns, connection, metadataScriptFactory);
            ComputedColumnsAdapter.Get(catalog, userTables, connection, metadataScriptFactory);
            DefaultConstraintsAdapter.Get(catalog, userTables, connection, metadataScriptFactory);
            CheckConstraintsAdapter.Get(catalog, userTables, connection, metadataScriptFactory);
            IdentityColumnsAdapter.Get(catalog, userTables, connection, metadataScriptFactory);
            IndexesAdapter.Get(catalog, userTables, connection, metadataScriptFactory);
        }
    }
}
