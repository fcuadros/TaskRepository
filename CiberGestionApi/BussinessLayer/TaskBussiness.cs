
using BussinessLayer.Interfaces;
using DataAccess.Interfaces;
using Entities.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class TaskBussiness : ITaskBussiness
    {
        private readonly ITaskData _data;
        public TaskBussiness(ITaskData data)
        {
            _data = data;
        }

        public async Task CreateTask(TaskDto request)
        {
          await _data.CreateTask(request);
        }

        public async Task DeleteTask(int id)
        {
            await _data.DeleteTask(id);
        }

        public async Task<TaskDto> getTask(int id)
        {
            return await _data.getTask(id);
        }
        public async Task<List<TaskDto>> ListTask()
        {
            return await _data.ListTask();
        }
    }
}
