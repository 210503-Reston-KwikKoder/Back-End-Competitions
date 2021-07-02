using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest.DTO
{
    public class LiveCompTestResultOutput:LiveCompTestResultInput
    {
        public LiveCompTestResultOutput() { }
        public string auth0Id { get; set; }
    }
}
