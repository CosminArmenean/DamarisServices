using Microsoft.AspNetCore.Mvc;

namespace Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces
{
    //
    // Summary:
    //     Provides an abstraction for the storage and management of user email addresses.
    //
    // Type parameters:
    //   TUser:
    //     The type encapsulating a user.
    public interface IUserPhoneStore<TUser> where TUser : class
    {
        //
        // Summary:
        //     Gets the user, if any, associated with the specified, normalized email address.
        //
        // Parameters:
        //   normalizedEmail:
        //     The normalized email address to return the user for.
        //
        //   cancellationToken:
        //     The System.Threading.CancellationToken used to propagate notifications that the
        //     operation should be canceled.
        //
        // Returns:
        //     The task object containing the results of the asynchronous lookup operation,
        //     the user if any associated with the specified normalized email address.
        Task<IActionResult> FindByPhoneAsync(string? phoneNumbern);

    }
}

