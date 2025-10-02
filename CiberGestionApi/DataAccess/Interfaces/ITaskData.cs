
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ITaskData
    {
        Task CreateTask(TaskDto request);
        Task DeleteTask(int request);
        Task<TaskDto> getTask(int id);
        Task<List<TaskDto>> ListTask();
    }
}
