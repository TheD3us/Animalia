using Administration.Models.Entities;
using System.Text.Json;

namespace Administration.Services
{
    public interface IAnimaliaApiService
    {
        // Events
        Task<List<Event>> GetEventsAsync();
        Task<Event?> GetEventAsync(int id);
        Task<Event?> CreateEventAsync(Event eventModel);
        Task<Event?> UpdateEventAsync(Event eventModel);
        Task<bool> DeleteEventAsync(int id);

        // Programs
        Task<List<ProgramEntity>> GetProgramsAsync();
        Task<ProgramEntity?> GetProgramAsync(int id);
        Task<ProgramEntity?> CreateProgramAsync(ProgramEntity programModel);
        Task<ProgramEntity?> UpdateProgramAsync(ProgramEntity programModel);
        Task<bool> DeleteProgramAsync(int id);

        // Trainings
        Task<List<Training>> GetTrainingsAsync();
        Task<Training?> GetTrainingAsync(int id);
        Task<Training?> CreateTrainingAsync(Training trainingModel);
        Task<Training?> UpdateTrainingAsync(Training trainingModel);
        Task<bool> DeleteTrainingAsync(int id);
    }

    public class AnimaliaApiService : IAnimaliaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AnimaliaApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["AnimaliaApi:BaseUrl"];
        }

        // Events Methods
        public async Task<List<Event>> GetEventsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/events");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Event>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Event>();
            }
            catch (Exception)
            {
                return new List<Event>();
            }
        }

        public async Task<Event?> GetEventAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/events/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Event>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Event?> CreateEventAsync(Event eventModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(eventModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/events", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Event>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Event?> UpdateEventAsync(Event eventModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(eventModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/events/{eventModel.Id}", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Event>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/events/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Programs Methods
        public async Task<List<ProgramEntity>> GetProgramsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/programs");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProgramEntity>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProgramEntity>();
            }
            catch (Exception)
            {
                return new List<ProgramEntity>();
            }
        }

        public async Task<ProgramEntity?> GetProgramAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/programs/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProgramEntity>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProgramEntity?> CreateProgramAsync(ProgramEntity programModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(programModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/programs", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProgramEntity>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProgramEntity?> UpdateProgramAsync(ProgramEntity programModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(programModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/programs/{programModel.Id}", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProgramEntity>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteProgramAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/programs/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Trainings Methods
        public async Task<List<Training>> GetTrainingsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/trainings");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Training>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Training>();
            }
            catch (Exception)
            {
                return new List<Training>();
            }
        }

        public async Task<Training?> GetTrainingAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/trainings/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Training>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Training?> CreateTrainingAsync(Training trainingModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(trainingModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/trainings", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Training>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Training?> UpdateTrainingAsync(Training trainingModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(trainingModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/trainings/{trainingModel.Id}", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Training>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteTrainingAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/trainings/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}