
using DataAccess.Interfaces;
using Entities.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
  
    public class TaskData : ITaskData
    {
        private readonly IConfiguration _config;

        public TaskData(IConfiguration config)
        {
            _config = config;
        }


        public async Task CreateTask(TaskDto request)
        {
            var result = new TaskDto();
            var connectionString = _config.GetConnectionString("Evaluacion");

            var exist = getTask(request.id);
            var cmd = "sp_insertTask";
            if (request.id != 0)
            {
                cmd = "sp_updateTask";

            }
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(cmd, connection);
            command.CommandType = CommandType.StoredProcedure;
            if (request.id != 0)
            {
                command.Parameters.AddWithValue("@id", request.id);
            }
            command.Parameters.AddWithValue("@tittle", request.Title);
            command.Parameters.AddWithValue("@descriptions", request.Description);
            command.Parameters.AddWithValue("@dueDate", request.DueDate);
            command.Parameters.AddWithValue("@state", request.state);
            await connection.OpenAsync();
  
            using var reader = await command.ExecuteReaderAsync();
           
        }

        public async Task DeleteTask(int id)
        {
            var result = new TaskDto();
            var connectionString = _config.GetConnectionString("Evaluacion");

            var exist = getTask(id);
            var cmd = "sp_deleteTask";
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(cmd, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
       
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
        }

        public async Task<TaskDto> getTask(int id)
        {
            try
            {
                var result = new TaskDto();
                var connectionString = _config.GetConnectionString("Evaluacion");

                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("sp_getTask", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result = (new TaskDto
                    {
                        Title = reader.GetString(0),
                        Description = reader.GetString(1),
                        DueDate = reader.GetDateTime(2),
                        state = reader.GetInt32(3)
                    });
                }
                return result;
            }
            catch (Exception ex)
            {

                var msg = ex.Message.ToString();
                throw;

            }
           
        }

        public async Task<List<TaskDto>> ListTask()
        {
            var result = new List<TaskDto>();
            var connectionString = _config.GetConnectionString("Evaluacion");

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("sp_listTask", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new TaskDto
                {
                    id= reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    DueDate = reader.GetDateTime(3),
                    state = reader.GetInt32(4)
                });
            }
            return result;
        }
    }
}
