using System.Threading.Tasks;
using SmartRegistry.Web.ViewModels;

namespace SmartRegistry.Web.Interfaces
{
    public interface IPatientHandler
    {
         Task<bool> AddPatient(PatientPayloadViewModel patient);
         Task<PatientPayloadViewModel> GetPatient(int id, bool includeAllDetails = false);

        Task<bool> UpdatePatient(PatientPayloadViewModel model);
    }
}
