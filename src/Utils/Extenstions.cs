using System.Security.Claims;
using System.Web;

namespace Wikirace.Utils;

public static class Extensions {

    /// <summary>
    /// Retrieves the value of a specified property from an object.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="obj">The object from which to retrieve the property value.</param>
    /// <param name="name">The name of the property.</param>
    /// <returns>The value of the specified property, or null if the property is not found or the value cannot be retrieved.</returns>
    public static T? PullProperty<T>(this object obj, string name) {
        var ty = obj.GetType();
        var prop = ty.GetProperty(name);
        return (T?)prop?.GetValue(obj);
    }

    /// <summary>
    /// Retrieves a service of the specified type from the request services of the HttpContext.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <param name="context">The HttpContext.</param>
    /// <returns>The service of the specified type, or null if the service is not found.</returns>
    public static T? GetService<T>(this HttpContext context) {
        return (T?)context.RequestServices.GetService(typeof(T));
    }

    /// <summary>
    /// Retrieves a service of the specified type from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <param name="provider">The service provider.</param>
    /// <returns>The service object of type <typeparamref name="T"/>, or <c>null</c> if there is no service of the specified type.</returns>
    public static T? GetService<T>(this IServiceProvider provider) {
        return (T?)provider.GetService(typeof(T));
    }

    /// <summary>
    /// Gets the user ID from the specified ClaimsPrincipal.
    /// </summary>
    /// <param name="user">The ClaimsPrincipal object.</param>
    /// <returns>The user ID as a string, or null if not found.</returns>
    public static string? GetUserId(this ClaimsPrincipal user) {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    /// <summary>
    /// Finds the index of the first element in the sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="source">The sequence to search.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// The zero-based index of the first occurrence of an element that satisfies the condition,
    /// or <c>null</c> if no such element is found.
    /// </returns>
    public static int? IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
        var index = 0;
        foreach (var item in source) {
            if (predicate(item)) {
                return index;
            }
            index++;
        }
        return null;
    }

    /// <summary>
    /// Returns the index of the first occurrence of an object of type T in the specified collection.
    /// </summary>
    /// <typeparam name="T">The type of object to search for.</typeparam>
    /// <param name="source">The collection to search in.</param>
    /// <returns>The index of the first occurrence of an object of type T, or null if no such object is found.</returns>
    public static int? IndexOfType<T>(this IEnumerable<object> source) {
        return source.IndexOf(o => o is T);
    }



    public static Uri AddQuery(this Uri uri, string name, string value) {
        var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

        httpValueCollection.Remove(name);
        httpValueCollection.Add(name, value);

        var ub = new UriBuilder(uri);
        ub.Query = httpValueCollection.ToString();

        return ub.Uri;
    }
}