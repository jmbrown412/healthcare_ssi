using Newtonsoft.Json;

namespace HealthSSI.Data
{
    public class BaseEntity
    {
        /// <summary>
        /// Helper method for serizling an object into json string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}