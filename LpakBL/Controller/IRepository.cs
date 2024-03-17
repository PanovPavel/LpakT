using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LpakBL.Model;

namespace LpakBL.Controller
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<List<T>> GetListAsync(); // получение всех объектов
        Task<T> GetAsync(Guid id); // получение одного объекта по id
        Task<T> AddAsync(T item); // добавление
        Task<T> UpdateAsync(T item); // обновление объекта
        Task RemoveAsync(Guid id); // удаление объекта по id
    }
}