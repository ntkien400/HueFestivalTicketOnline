﻿using HueFestivalTicketOnline.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository.IRepository
{
    public interface IDetailFesLocationRepository : IGenericRepository<DetailFesLocation>
    {
        public bool CheckDate(string s1, string s2);
    }
}
