using System.IO;
using System.Threading.Tasks;

namespace MasterDetailTemplate.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
