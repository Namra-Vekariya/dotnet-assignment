using Microsoft.AspNetCore.Mvc;
using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.JsonPatch;
using CollegeApp.Data;
using Microsoft.EntityFrameworkCore;
namespace CollegeApp.Controllers{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dbContext;

        public StudentController(ILogger<StudentController> logger ,CollegeDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

        }
        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<Student>> GetStudents()
        {
            //  return CollegeRepository.Students;
            // var students = new List<StudentDTO>();
            // foreach(var item in CollegeRepository.Students){
            //     StudentDTO obj = new StudentDTO(){
            //         Id = item.Id,
            //         StudentName = item.StudentName,
            //         Address = item.Address,
            //         Email = item.Email,
            //     };
            //     students.Add(obj);
            _logger.LogInformation("Get student method started");
            // }
            var students = await _dbContext.Students.ToListAsync();
            // var students = await _dbContext.Students.Select(s => new StudentDTO(){
            //     Id = s.Id,
            //     StudentName = s.StudentName,
            //     Address = s.Address,
            //     Email = s.Email,
                // DOB = s.DOB.ToShortDateString(),
            // }).ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(200,Type=typeof(Student))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            if (id < 0 ){
                _logger.LogWarning("Bad request");
                return BadRequest("id should be positive");
            }
            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.Id == id);
            if (student == null)
            {
                _logger.LogError("Student not found with id: {id}", id);
                return NotFound();
            }
            var studentDTO = new StudentDTO {
                Id = student.Id,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email,
                DOB = student.DOB
            };
            return Ok(student);  // Return a single student
            // return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}")]
        public async Task<ActionResult<Student>> GetStudentByname(string name)
        {
            // return 
            var student  =await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentName == name);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);  
            // Return a single student
        }

        // create a student
        [HttpPost] 
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO model){
            // if we donot use model validation
                // if(!ModelState.IsValid){
                //     return BadRequest(ModelState);
                // }
                // if(model == null){
                //     return BadRequest("Student data is null");
                // }

                //adding error message to model state 
                // if(model.AdmissionDate > DateTime.Now){
                //     ModelState.AddModelError("AdmissionDate", "Admission date cannot be in the future");
                //     return BadRequest(ModelState);
                // }

                // increment by one
                // int newId = _dbContext.Students.Count > 0 ? _dbContext.Students.Max(s => s.Id) + 1 : 1;
                var student = new Student
                {
                    // Id = newId,
                    StudentName = model.StudentName,
                    Email = model.Email,
                    Address = model.Address,
                    DOB = model.DOB
                };
                await _dbContext.Students.AddAsync(student);
                // after adding we need to save changes
                await _dbContext.SaveChangesAsync();
                model.Id = student.Id; 
                return CreatedAtRoute("GetStudentById", new { id = model.Id }, model); // Return a 201 Created response with the location of the new student
                // return Ok(model); 
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> UpdateStudent([FromBody] StudentDTO model){
            if(model == null || model.Id <= 0){
                return BadRequest("Student data is null");
            }
            var existingStudent =await _dbContext.Students.AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefaultAsync();
            if(existingStudent == null){
                return NotFound("Student not found");
            }
            var newRecord = new Student(){
                // id is already there in db but due to it is no tracking it works 
                Id = existingStudent.Id,
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address,
                DOB = model.DOB
            };
             _dbContext.Students.Update(newRecord);
            // existingStudent.StudentName = model.StudentName;
            // existingStudent.Email = model.Email;
            // existingStudent.Address = model.Address;
            // existingStudent.DOB =model.DOB;
            await _dbContext.SaveChangesAsync();
            return Ok(model); // Return a success response with the updated student data
        }

        // update by patch
         [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async  Task<ActionResult<StudentDTO>> UpdateStudentPartial (int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument){
            if(patchDocument == null || id <= 0){
                return BadRequest("Student data is null");
            }
            var existingStudent =await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
            if(existingStudent == null){
                return NotFound("Student not found");
            }
            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };
            patchDocument.ApplyTo(studentDTO,ModelState);

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;
            await _dbContext.SaveChangesAsync();
            return NoContent(); // Return a success response with the updated student data
        }
        // Delete a student by ID
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> deleteStudent(int id)
        {
            var student =await  _dbContext.Students.FirstOrDefaultAsync(n => n.Id == id);
            if (student == null)
            {
                return NotFound();  // Return a 404 if the student is not found
            }
            _dbContext.Students.Remove(student);  // Remove the single student
            return Ok(true);  // Return a success response
        }

        [HttpDelete("Delete/{id:int}",Name = "DeleteStudentById")]
        public async Task<ActionResult<bool>> DeleteStudentById(int id)
        {
            if(id <= 0){
                return BadRequest("Invalid student ID");
            }
            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.Id == id);
            if (student == null)
            {
                return NotFound();  // Return a 404 if the student is not found
            }
            _dbContext.Students.Remove(student);  // Remove the single student
            await _dbContext.SaveChangesAsync();
            return Ok(true);  // Return a success response
    }
}}