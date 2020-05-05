using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PresidentsApi.Models;

namespace PresidentsApi.Services
{
    public class PresidentDataService
    {
        private const string AllPartyKey = "all";

        private IList<PresidentInfo> _datasource;
        private IList<PresidentInfo> Datasource
        {
            get
            {
                if (_datasource == null)
                    _datasource = JsonConvert.DeserializeObject<IList<PresidentInfo>>(File.ReadAllText(@"./presidents_data.json"));

                return _datasource;
            }
        }

        public IList<PresidentInfo> GetAllPresidents()
        {
            return Datasource;
        }

        public IList<PresidentInfo> GetAllPresidentsByParty(string partyName)
        {
            if (string.Compare(partyName, AllPartyKey, ignoreCase: true) == 0)
                return Datasource;

            return Datasource.Where(x => string.Compare(x.Party, partyName, ignoreCase: true) == 0)
                .ToList();
        }

        public IList<PresidentInfo> GetLivingPresidents()
        {
            return Datasource.Where(x => x.DeathYear.HasValue == false).ToList();
        }

        public IList<PresidentInfo> GetPresidentsWhoDiedInOffice()
        {
            return Datasource.Where(x => x.DeathYear.HasValue
                && x.TermEnd.HasValue
                && x.TermEnd.Value.Year == x.DeathYear.Value)
                .ToList();
        }
    }
}