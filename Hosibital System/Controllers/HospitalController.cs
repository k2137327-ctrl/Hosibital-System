using HospitalSystem.Data;
using HospitalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hosibital_System.Controllers
{
    public class HospitalController : Controller
    {
         
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

         
        public async Task<IActionResult> Doctors(string? searchName, string? specialization, int page = 1)
        {
            int pageSize = 3;  
            var query = _context.Doctors.AsQueryable();

             
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(d => d.Name.Contains(searchName));
            }

             
            if (!string.IsNullOrWhiteSpace(specialization))
            {
                query = query.Where(d => d.Specialization == specialization);
            }

             
            int totalDoctors = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);
            if (page < 1) page = 1;

            var doctors = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

             
            ViewBag.Specializations = await _context.Doctors
                .Select(d => d.Specialization)
                .Distinct()
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchName = searchName;
            ViewBag.Specialization = specialization;

            return View(doctors);
        }

         
        [HttpGet]
        public async Task<IActionResult> Book(int doctorId)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null) return NotFound();

            ViewBag.Doctor = doctor;
            ViewBag.TimeSlots = GetTimeSlots();

            var appointment = new Appointment
            {
                DoctorId = doctorId,
                AppointmentDate = DateTime.Today
            };

            return View(appointment);
        }

         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(Appointment appointment)
        {
             
            if (appointment.AppointmentDate.DayOfWeek == DayOfWeek.Friday ||
                appointment.AppointmentDate.DayOfWeek == DayOfWeek.Saturday)
            {
                ModelState.AddModelError("AppointmentDate", "Appointments are only available from Sunday to Thursday.");
            }

             
            bool isDoubleBooked = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == appointment.DoctorId &&
                a.AppointmentDate.Date == appointment.AppointmentDate.Date &&
                a.AppointmentTime == appointment.AppointmentTime);

            if (isDoubleBooked)
            {
                ModelState.AddModelError(string.Empty, "This doctor is already booked at this date and time. Please choose another time.");
            }

            if (ModelState.IsValid)
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Appointments));  
            }

             
            ViewBag.Doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
            ViewBag.TimeSlots = GetTimeSlots();
            return View(appointment);
        }

         
        public async Task<IActionResult> Appointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

         
        private List<string> GetTimeSlots()
        {
            var slots = new List<string>();
            TimeSpan current = new TimeSpan(9, 0, 0);   
            TimeSpan end = new TimeSpan(16, 30, 0);     

            while (current <= end)
            {
                DateTime dt = DateTime.Today.Add(current);
                slots.Add(dt.ToString("hh:mm tt"));
                current = current.Add(TimeSpan.FromMinutes(30));
            }
            return slots;
        }
    }
}