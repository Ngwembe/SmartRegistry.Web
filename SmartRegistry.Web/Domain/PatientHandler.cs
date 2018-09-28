using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartRegistry.Web.Interfaces;
using SmartRegistry.Web.ViewModels;

namespace SmartRegistry.Web.Domain
{
    public class PatientHandler :IPatientHandler
    {
        private readonly IApiAccessor _apiAccessor;
        //private HttpClient client;
        public PatientHandler(IApiAccessor apiAccessor)
        {
            _apiAccessor = apiAccessor;

            //url: 'https://admissions-dot-medipark-hospital.appspot.com/v1/Patient/create',

            //client = new HttpClient()
            //{
            //    BaseAddress = new Uri("https://admissions-dot-medipark-hospital.appspot.com/v1/")
            //};
            //client.DefaultRequestHeaders.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> AddPatient(PatientPayloadViewModel patient)
        {
            HttpResponseMessage res = await _apiAccessor.GetHttpClient().PostAsJsonAsync<PatientPayloadViewModel>("Patient/create", patient);

            if (res.IsSuccessStatusCode)
            {
                var empResponse = res.Content.ReadAsStringAsync().Result;

                var newPatient = JsonConvert.DeserializeObject<PatientPayloadViewModel>(empResponse);

                return true;
            }

            return false;
        }

        public async Task<PatientPayloadViewModel> GetPatient(int id, bool includeAllDetails = false)
        {
            try
            {
                var response = await _apiAccessor.GetHttpClient().GetAsync($"Patient/{id}/{includeAllDetails}");

                if (response.IsSuccessStatusCode)
                {
                    var patient = await response.Content.ReadAsAsync<PatientPayloadViewModel>();
                    return patient;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdatePatient(PatientPayloadViewModel patient)
        {
            HttpResponseMessage res = await _apiAccessor.GetHttpClient().PutAsJsonAsync<PatientPayloadViewModel>("Patient/update", patient);

            if (res.IsSuccessStatusCode)
            {
                var empResponse = res.Content.ReadAsStringAsync().Result;

                var newPatient = JsonConvert.DeserializeObject<PatientPayloadViewModel>(empResponse);

                return true;
            }

            return false;
        }
    }
}
