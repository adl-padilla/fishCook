namespace SDL2.SDL_Extensions;

internal class SpriteCollection : List<Sprite>, IDisposable
{
    public SpriteCollection(TextureCollection textures)
    {
        textures.ForEach(x => Add(new Sprite(x)));
    }

    public void Dispose()
    {
        ForEach(x => x.Dispose());
        //throw new NotImplementedException();
    }
}
