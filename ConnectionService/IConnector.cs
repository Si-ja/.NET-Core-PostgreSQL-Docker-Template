using dockerapi.Models;
using System;
using System.Collections;

namespace dockerapi.ConnectionService
{
    public interface IConnector
    {
        public void OpenConnection();
        public void CloseConnection();
        public String CheckDBVersion();
        public Music QuerySelectRandomSong(string SQLStatementFile);
        public void QueryWithParametersInsert(string SQLStatementFile, string[] ExpectedParameters, Hashtable SQLParameters, Hashtable ParameterTypes);
    }
}