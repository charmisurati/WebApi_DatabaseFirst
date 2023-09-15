using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi_DatabaseFirst.DBContext;
using WebApi_DatabaseFirst.Filters;

namespace WebApi_DatabaseFirst.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        private readonly  DatabaseFirstCRUDEntities3 dbfirst;

        public ValuesController()
        {
            this.dbfirst = new DatabaseFirstCRUDEntities3();
        }

        // GET api/values
        [Authorize]
        [CustomAuthFilter]
        [ActionName("GetStudents")]
        public IEnumerable<StudentData> Get()
        {
            return this.dbfirst.StudentDatas.ToList();
            
        }

        // GET api/values/5
        [HttpPost]
        [ActionName("CreateStudent")]
        public int CreateStudent(StudentData sd)
        {
            this.dbfirst.StudentDatas.Add(sd);
            this.dbfirst.SaveChanges();
;            return sd.Id;
        }

        [ActionName("GetStudentById")]
        public async Task<StudentData> GetStudentById(int id)
        {
            return await this.dbfirst.StudentDatas.Where(x => x.Id == id).FirstAsync();

        }

        [HttpPut]
        [ActionName("UpdateStudent")]
        public int UpdateEmployee(StudentData sd)
        {
            StudentData existingrecord = this.dbfirst.StudentDatas.Where(x => x.Id == sd.Id).FirstOrDefault();
            this.dbfirst.Entry(existingrecord).CurrentValues.SetValues(sd);
            this.dbfirst.SaveChanges();
            return sd.Id;
        }


        // DELETE api/values/5
        [HttpDelete]
        [ActionName("DeleteStudent")]
        public void Delete(int id)
        {
            var remove = this.dbfirst.StudentDatas.Where(x => x.Id == id).FirstOrDefault();
            this.dbfirst.StudentDatas.Remove(remove);
            this.dbfirst.SaveChanges();
        }
    }
}
