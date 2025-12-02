using Microsoft.AspNetCore.Mvc;
using AIOS.Models;
using AIOS.Repositories;

namespace AIOS.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsApiController : ControllerBase
    {
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentsApiController(AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        // GET: api/appointments
        [HttpGet]
        public async Task<ActionResult<List<Appointment>>> GetAll()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/appointments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetById(string id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(id);

                if (appointment == null)
                    return NotFound(new { error = "Appointment not found" });

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/appointments/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<Appointment>>> GetByCustomerId(string customerId)
        {
            try
            {
                var appointments = await _appointmentRepository.GetByCustomerIdAsync(customerId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> Create([FromBody] Appointment appointment)
        {
            try
            {
                var createdAppointment = await _appointmentRepository.CreateAsync(appointment);
                return CreatedAtAction(nameof(GetById), new { id = createdAppointment.Id }, createdAppointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/appointments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Appointment appointment)
        {
            try
            {
                if (!MongoDB.Bson.ObjectId.TryParse(id, out _))
                {
                    return BadRequest(new { error = "Invalid appointment ID format" });
                }

                var existingAppointment = await _appointmentRepository.GetByIdAsync(id);

                if (existingAppointment == null)
                    return NotFound(new { error = "Appointment not found" });

                appointment.Id = id;
                appointment.CreatedAt = existingAppointment.CreatedAt;

                var success = await _appointmentRepository.UpdateAsync(id, appointment);

                if (!success)
                    return StatusCode(500, new { error = "Failed to update appointment" });

                return Ok(new { message = "Appointment updated successfully", appointment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/appointments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(id);

                if (appointment == null)
                    return NotFound(new { error = "Appointment not found" });

                await _appointmentRepository.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}