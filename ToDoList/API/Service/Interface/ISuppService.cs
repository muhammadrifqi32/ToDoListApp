﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace API.Service.Interface
{
    public interface ISuppService
    {
        IEnumerable<Supp> Get();
        Task<IEnumerable<Supp>> Get(int Id);
        int Create(SuppVM suppVM);
        int Update(int Id, SuppVM suppVM);
        int Delete(int Id);
        Task<SuppVM> PageSearch(string keyword, int pageSize, int pageNumber);
    }
}
