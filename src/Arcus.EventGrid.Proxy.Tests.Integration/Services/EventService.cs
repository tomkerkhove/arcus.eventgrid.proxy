﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Arcus.EventGrid.Proxy.Tests.Integration.Services
{
    public class EventService : Service
    {
        public EventService(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<HttpResponseMessage> EmitAsync(object eventPayload, string eventType, string eventId = "", string eventTimestamp = "", string eventSubject = "", string dataVersion = "")
        {
            var rawEventPayload = JsonConvert.SerializeObject(eventPayload);
            return await EmitAsync(rawEventPayload, eventType, eventId);
        }

        public async Task<HttpResponseMessage> EmitAsync(string eventPayload, string eventType, string eventId = "", string eventTimestamp = "", string eventSubject = "", string dataVersion = "")
        {
            var url = BaseUrl.AppendPathSegments("events", eventType);

            if (string.IsNullOrWhiteSpace(eventId) == false)
            {
                url = url.SetQueryParam("eventId", eventId);
            }

            if (string.IsNullOrWhiteSpace(eventTimestamp) == false)
            {
                url = url.SetQueryParam("eventTimestamp", eventTimestamp);
            }

            if (string.IsNullOrWhiteSpace(eventSubject) == false)
            {
                url = url.SetQueryParam("eventSubject", eventSubject);
            }

            if (string.IsNullOrWhiteSpace(dataVersion) == false)
            {
                url = url.SetQueryParam("dataVersion", dataVersion);
            }

            var response = await HttpClient.PostAsync(url, new StringContent(eventPayload, Encoding.UTF8, "application/json"));
            return response;
        }
    }
}