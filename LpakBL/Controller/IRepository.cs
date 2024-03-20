using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LpakBL.Controller
{
    /// <summary>
    /// Интерфейс для асинхронного взаимодействия с БД 
    /// </summary>
    /// <typeparam name="T">Тип объекта с которым взаимодействует БД</typeparam>
    public interface IRepositoryAsync<T> where T : class
    {
        
        /// <summary>
        /// Получение списка объектов типа T
        /// </summary>
        /// <returns>Список всех объектов</returns>
        Task<List<T>> GetListAsync();
        
        /// <summary>
        /// Получение объекта типа T по id
        /// </summary>
        /// <param name="id">Id получаймого объекта</param>
        /// <returns>Полученный объект из БД</returns>
        Task<T> GetAsync(Guid id); 
        
        /// <summary>
        /// Добавление нового объекта типа T в базу данных
        /// </summary>
        /// <param name="item">добавляймый в БД объект</param>
        /// <returns>объект который был добавлен</returns>
        Task<T> AddAsync(T item);
        
        /// <summary>
        /// Изменение значений объекта типа T в базе данных
        /// </summary>
        /// <param name="item">объект который был изменён</param>
        /// <returns></returns>
        Task<T> UpdateAsync(T item);
        
        /// <summary>
        /// Удаление объекта типа T из базы данных по id
        /// </summary>
        /// <param name="id">id удаляймого объекта</param>
        /// <returns>Объект который был удалён</returns>
        Task RemoveAsync(Guid id); // удаление объекта по id
    }
}