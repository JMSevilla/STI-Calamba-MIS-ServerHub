using System.Text.Json;

namespace sti_sys_backend.Utilization;

public class WorldTimeAPI
{
    string apiUrl = "https://timeapi.io/api/Time/current/zone?timeZone=Asia/Manila";

    public async Task<DateTime> ConfigureDateTime()
    {
        using (HttpClient httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                WorldTimeResponsev2 worldTimeResponsev2 =
                    JsonSerializer.Deserialize<WorldTimeResponsev2>(responseContent);
                if (DateTimeOffset.TryParse(worldTimeResponsev2.dateTime, out DateTimeOffset dateTimeOffset))
                {
                    return dateTimeOffset.DateTime;
                }
            }
            return DateTime.Today;
        }
    }

    public async Task<TimeSpan> ConfigureTimeSpan()
    {
        using (HttpClient httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                WorldTimeResponsev2 worldTimeResponsev2 =
                    JsonSerializer.Deserialize<WorldTimeResponsev2>(responseContent);
                if (DateTimeOffset.TryParse(worldTimeResponsev2.dateTime, out DateTimeOffset dateTimeOffset))
                {
                    TimeSpan currentTime = dateTimeOffset.TimeOfDay;
                }
            }

            return TimeSpan.Zero;
        }
    }
}