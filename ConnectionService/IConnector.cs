using dockerapi.Models;
using dockerapi.Scripts.InformationManipulation;
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
        public int CheckMusicGenresQuery(IGenresChecker genresChecker, string SQLStatementFile, string genreType);
        public void QueryWithParametersInsert(string SQLStatementFile, string[] ExpectedParameters, Hashtable SQLParameters, Hashtable ParameterTypes);
    }
}