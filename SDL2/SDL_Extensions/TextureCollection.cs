
namespace SDL2.SDL_Extensions;

public class TextureCollection(IEnumerable<Texture> source) : List<Texture>(source), IDisposable
{
    public void Dispose() => ForEach(x => x.Dispose());
}
