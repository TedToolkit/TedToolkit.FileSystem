// -----------------------------------------------------------------------
// <copyright file="IsExternalInit.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

#if NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
namespace System.Runtime.CompilerServices;

/// <summary>
/// Provides init-only support for target frameworks that do not define the runtime type.
/// </summary>
internal static class IsExternalInit;
#endif