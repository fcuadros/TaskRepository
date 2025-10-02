using CiberGestionApi.Entities;
using Microsoft.AspNetCore.Mvc;
using BussinessLayer.Interfaces;
using Entities.Entities;

namespace CiberGestionApi.Controllers
{
    public class TaskController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ITaskBussiness _taskBussiness;

        public TaskController(IConfiguration config, ITaskBussiness taskBussiness)
        {
            _config = config;
            _taskBussiness = taskBussiness;
        }

        [HttpPost("create-task")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto task)
        {
            try
            {
                await _taskBussiness.CreateTask(task);
                return Ok(new { message = "Tarea creada exitosamente", task });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la tarea", error = ex.Message });
            }
        }

        [HttpGet("get-task/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var task = await _taskBussiness.getTask(id);
                if (task == null)
                    return NotFound(new { message = "Tarea no encontrada" });

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener la tarea", error = ex.Message });
            }
        }

        [HttpGet("list-tasks")]
        public async Task<IActionResult> ListTasks()
        {
            try
            {
                var tasks = await _taskBussiness.ListTask();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al listar las tareas", error = ex.Message });
            }
        }

        [HttpDelete("delete-task/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskBussiness.DeleteTask(id);
                return Ok(new { message = "Tarea eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la tarea", error = ex.Message });
            }
        }

        [HttpPut("update-task/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto task)
        {
            try
            {
                // Primero verifica si la tarea existe
                var existingTask = await _taskBussiness.getTask(id);
                if (existingTask == null)
                    return NotFound(new { message = "Tarea no encontrada" });

              
                task.id = id;

             
                 await _taskBussiness.CreateTask(task);

                return Ok(new { message = "Tarea actualizada exitosamente", task });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la tarea", error = ex.Message });
            }
        }
    }
}
