using static SDL2.SDL;
using static SDL2.SDL_image;
namespace SDL2.SDL_Extensions;

public class Renderer(nint value = 0) : SDLPointer<Renderer>(value), IDisposable
{
    public void Dispose()
    {
        SDL_DestroyRenderer(Value);
        //SDL_LogInfo(0,$"Destroying Renderer {Value}");
        SDL_LogInfo(0, $"{DateTime.Now.TimeOfDay} [{nameof(Renderer)}.{nameof(Dispose)}] Destroying {Convert.ToString(Value, 16)}");

        GC.SuppressFinalize(this);
    }

    public int SetRenderDrawColor(SDL_Color color)
    {
        var rv = SDL_SetRenderDrawColor(Value, color.r, color.g, color.b, color.a);
        // Sets the color that the screen will be cleared with.
        if (rv < 0)
        {
            SDL_LogInfo(0, $"There was an issue with setting the render draw color. {SDL_GetError()}");
        }
        return rv;
    }

    public int RenderClear()
    {
        var rv = SDL_RenderClear(Value);
        // Clears the current render surface.
        if (rv < 0)
        {
            SDL_LogInfo(0, $"There was an issue with clearing the render surface. {SDL_GetError()}");
        }
        return rv;
    }

    public int RenderCopy(Texture texture, ref SDL_Rect sourceRect, ref SDL_Rect destRect)
    {
        var rv = SDL_RenderCopy(Value, texture.Value, ref sourceRect, ref destRect);
        if (rv < 0)
        {
            SDL_LogInfo(0, $"There was an issue rendering the image. {SDL_GetError()}");
        }
        return rv;
    }

    public void RenderPresent()
    {
        // Switches out the currently presented render surface with the one we just did work on.
        SDL_RenderPresent(Value);
    }

    public int RenderCopyScale(Texture texture, SDL_Point location, double scale = 1.0)
    {
        SDL_Point size;
        _ = SDL_QueryTexture(texture.Value, out uint format, out int access, out size.x, out size.y);
        var sourceRect = new SDL_Rect { x = 0, y = 0, w = size.x, h = size.y };
        var destRect = new SDL_Rect { x = location.x, y = location.y, w = (int)(size.x * scale), h = (int)(size.y * scale) };

        var rv = SDL_RenderCopy(Value, texture.Value, ref sourceRect, ref destRect);
        if (rv < 0)
        {
            SDL_LogInfo(0, $"There was an issue rendering the image. {SDL_GetError()}");
        }
        return rv;

    }

    public Texture LoadTexture(string fullName)
    {
        return new(IMG_LoadTexture(Value, fullName));
    }

    public int RenderCopyEx(Texture texture, ref SDL_Rect r1, ref SDL_Rect r2, float angle, SDL_Point center, SDL_RendererFlip flip)
    {
        return SDL_RenderCopyEx(Value, texture.Value, ref r1, ref r2, angle, ref center, flip);

    }
}

