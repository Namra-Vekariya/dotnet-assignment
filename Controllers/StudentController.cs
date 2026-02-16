using Microsoft.AspNetCore.Mvc;
using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.JsonPatch;
using CollegeApp.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CollegeApp.Data.Repository;
using Routing.Data.Repository;
namespace CollegeApp.Controllers{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        // private readonly CollegeDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        public StudentController(ILogger<StudentController> logger ,CollegeDBContext dbContext,IMapper mapper,IStudentRepository studentRepository)
        {
            _logger = logger;
            // _dbContext = dbContext;
            _studentRepository = studentRepository;
            _mapper = mapper;

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
            var students = await _studentRepository.GetAllAsync();
            var StudentDTOData = _mapper.Map<List<StudentDTO>>(students);
            // var students = await _dbContext.Students.Select(s => new StudentDTO(){
            //     Id = s.Id,
            //     StudentName = s.StudentName,
            //     Address = s.Address,
            //     Email = s.Email,
                // DOB = s.DOB.ToShortDateString(),
            // }).ToListAsync();
            return Ok(StudentDTOData);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(200,Type=typeof(StudentDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            if (id < 0 ){
                _logger.LogWarning("Bad request");
                return BadRequest("id should be positive");
            }
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogError("Student not found with id: {id}", id);
                return NotFound();
            }
            // var studentDTO = new StudentDTO {
            //     Id = student.Id,
            //     StudentName = student.StudentName,
            //     Address = student.Address,
            //     Email = student.Email,
            //     DOB = student.DOB
            // };
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(studentDTO);  // Return a single student
            // return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}")]
        public async Task<ActionResult<Student>> GetStudentByname(string name)
        {
            // return 
            var student  =await _studentRepository.GetByNameAsync(name);
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
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO dto){
            // if we donot use model validation
                // if(!ModelState.IsValid){
                //     return BadRequest(ModelState);
                // }
                if(dto == null){
                    return BadRequest("Student data is null");
                }

                //adding error message to model state 
                // if(model.AdmissionDate > DateTime.Now){
                //     ModelState.AddModelError("AdmissionDate", "Admission date cannot be in the future");
                //     return BadRequest(ModelState);
                // }

                // increment by one
                // int newId = _dbContext.Students.Count > 0 ? _dbContext.Students.Max(s => s.Id) + 1 : 1;
                // var student = new Student
                // {
                //     // Id = newId,
                //     StudentName = dto.StudentName,
                //     Email = dto.Email,
                //     Address = dto.Address,
                //     DOB = dto.DOB
                // };
                
                Student student = _mapper.Map<Student>(dto);

                await _studentRepository.CreateAsync(student);
                // await _dbContext.Students.AddAsync(student);
                // after adding we need to save changes
                // await _dbContext.SaveChangesAsync();
                dto.Id = student.Id; 
                return CreatedAtRoute("GetStudentById", new { id = dto.Id }, dto); // Return a 201 Created response with the location of the new student
                // return Ok(model); 
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> UpdateStudent([FromBody] StudentDTO dto){
            if(dto == null || dto.Id <= 0){
                return BadRequest("Student data is null");
            }
            var existingStudent =await _studentRepository.GetByIdAsync(dto.Id,true);
            if(existingStudent == null){
                return NotFound("Student not found");
            }
            // var newRecord = new Student(){
            //     // id is already there in db but due to it is no tracking it works 
            //     Id = existingStudent.Id,
            //     StudentName = model.StudentName,
            //     Email = model.Email,
            //     Address = model.Address,
            //     DOB = model.DOB
            // };
            var newRecord = _mapper.Map<Student>(dto);
            await  _studentRepository.UpdateAsync(newRecord);
            // existingStudent.StudentName = model.StudentName;
            // existingStudent.Email = model.Email;
            // existingStudent.Address = model.Address;
            // existingStudent.DOB =model.DOB;
            return Ok(newRecord); // Return a success response with the updated student data
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

            var existingStudent =await _studentRepository.GetByIdAsync(id,true);

            if(existingStudent == null){
                return NotFound("Student not found");
            }
            // var studentDTO = new StudentDTO
            // {
            //     Id = existingStudent.Id,
            //     StudentName = existingStudent.StudentName,
            //     Email = existingStudent.Email,
            //     Address = existingStudent.Address
            // };
            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

            patchDocument.ApplyTo(studentDTO,ModelState);

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

             _mapper.Map(studentDTO, existingStudent);
            // existingStudent.StudentName = studentDTO.StudentName;
            // existingStudent.Email = studentDTO.Email;
            // existingStudent.Address = studentDTO.Address;
            await _studentRepository.UpdateAsync(existingStudent);
            return NoContent(); // Return a success response with the updated student data
        }
        // Delete a student by ID
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> deleteStudent(int id)
        {
            var student =await  _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();  // Return a 404 if the student is not found
            }
            // _dbContext.Students.Remove(student); 
             // Remove the single student
             await _studentRepository.DeleteAsync(student);
            return Ok(true);  // Return a success response
        }

        [HttpDelete("Delete/{id:int}",Name = "DeleteStudentById")]
        public async Task<ActionResult<bool>> DeleteStudentById(int id)
        {
            if(id <= 0){
                return BadRequest("Invalid student ID");
            }
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();  // Return a 404 if the student is not found
            }
            await _studentRepository.DeleteAsync(student);
            return Ok(true);  // Return a success response
    }
}}