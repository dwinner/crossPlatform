using System.Diagnostics.CodeAnalysis;

namespace Recipes.Client.Core;

public sealed class Result<TSuccess>
   where TSuccess : notnull
{
   public Result(TSuccess? data, string? errorCode, string? errorData, Exception? exception, bool isSuccess)
   {
      Data = data;
      ErrorCode = errorCode;
      ErrorData = errorData;
      Exception = exception;
      IsSuccess = isSuccess;
   }

   public TSuccess? Data { get; }

   public string? ErrorCode { get; }

   public string? ErrorData { get; }

   public Exception? Exception { get; }

   [MemberNotNullWhen(true, nameof(Data))]
   [MemberNotNullWhen(false, nameof(ErrorCode))]
   [MemberNotNullWhen(false, nameof(ErrorData))]
   public bool IsSuccess { get; }

   public static Result<TSuccess> Success(TSuccess data) =>
      new(data, null, null, null, true);

   public static Result<TSuccess> Success() =>
      new(default, null, null, null, true);

   public static Result<TSuccess> Fail(string errorCode, string? errorData = null, Exception? exception = null)
      => new(default, errorCode, errorData, exception, false);

   public static Result<TSuccess> Fail(Exception exception)
      => new(default, nameof(exception), exception.Message, exception, false);
}