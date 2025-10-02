using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interfaces
{
    public interface ITaskBussiness
    {
       Task CreateTask(TaskDto request);
       Task DeleteTask(int id);
       Task<TaskDto> getTask(int id);
       Task<List<TaskDto>> ListTask();
    }
}
