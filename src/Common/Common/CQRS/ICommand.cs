﻿namespace Common.CQRS
{
    public interface ICommand<out TResponse> where TResponse : notnull
    {
    }
}
