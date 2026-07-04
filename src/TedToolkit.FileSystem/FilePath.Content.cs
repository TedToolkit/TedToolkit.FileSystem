// -----------------------------------------------------------------------
// <copyright file="FilePath.Content.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text;

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides content read and write helpers for file paths.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Reads all text from the file using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllText(string)" />.</remarks>
    /// <returns>The file contents.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ReadAllText()
    {
        return File.ReadAllText(FullName);
    }

    /// <summary>
    /// Reads all text from the file using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllText(string,System.Text.Encoding)" />.</remarks>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>The file contents.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ReadAllText(Encoding encoding)
    {
        return File.ReadAllText(FullName, encoding);
    }

    /// <summary>
    /// Reads all bytes from the file.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllBytes(string)" />.</remarks>
    /// <returns>The file bytes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte[] ReadAllBytes()
    {
        return File.ReadAllBytes(FullName);
    }

    /// <summary>
    /// Reads all lines from the file using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllLines(string)" />.</remarks>
    /// <returns>The file lines.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string[] ReadAllLines()
    {
        return File.ReadAllLines(FullName);
    }

    /// <summary>
    /// Reads all lines from the file using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllLines(string,System.Text.Encoding)" />.</remarks>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>The file lines.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string[] ReadAllLines(Encoding encoding)
    {
        return File.ReadAllLines(FullName, encoding);
    }

    /// <summary>
    /// Reads the file line by line using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadLines(string)" />.</remarks>
    /// <returns>The file lines.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<string> ReadLines()
    {
        return File.ReadLines(FullName);
    }

    /// <summary>
    /// Reads the file line by line using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadLines(string,System.Text.Encoding)" />.</remarks>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>The file lines.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<string> ReadLines(Encoding encoding)
    {
        return File.ReadLines(FullName, encoding);
    }

    /// <summary>
    /// Writes all text to the file using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.WriteAllText(string,string)" />.</remarks>
    /// <param name="contents">The text to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAllText(string contents)
    {
        File.WriteAllText(FullName, contents);
    }

    /// <summary>
    /// Writes all text to the file using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.WriteAllText(string,string,System.Text.Encoding)" />.</remarks>
    /// <param name="contents">The text to write.</param>
    /// <param name="encoding">The encoding to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAllText(string contents, Encoding encoding)
    {
        File.WriteAllText(FullName, contents, encoding);
    }

    /// <summary>
    /// Writes all bytes to the file.
    /// </summary>
    /// <remarks>Wraps <see cref="File.WriteAllBytes(string,byte[])" />.</remarks>
    /// <param name="bytes">The bytes to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAllBytes(byte[] bytes)
    {
        File.WriteAllBytes(FullName, bytes);
    }

    /// <summary>
    /// Writes all lines to the file using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.WriteAllLines(string,System.Collections.Generic.IEnumerable{string})" />.</remarks>
    /// <param name="contents">The lines to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAllLines(IEnumerable<string> contents)
    {
        File.WriteAllLines(FullName, contents);
    }

    /// <summary>
    /// Writes all lines to the file using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.WriteAllLines(string,System.Collections.Generic.IEnumerable{string},System.Text.Encoding)" />.</remarks>
    /// <param name="contents">The lines to write.</param>
    /// <param name="encoding">The encoding to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAllLines(IEnumerable<string> contents, Encoding encoding)
    {
        File.WriteAllLines(FullName, contents, encoding);
    }

    /// <summary>
    /// Appends all text to the file using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.AppendAllText(string,string)" />.</remarks>
    /// <param name="contents">The text to append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendAllText(string contents)
    {
        File.AppendAllText(FullName, contents);
    }

    /// <summary>
    /// Appends all text to the file using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.AppendAllText(string,string,System.Text.Encoding)" />.</remarks>
    /// <param name="contents">The text to append.</param>
    /// <param name="encoding">The encoding to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendAllText(string contents, Encoding encoding)
    {
        File.AppendAllText(FullName, contents, encoding);
    }

#if NET9_0_OR_GREATER
    /// <summary>
    /// Appends all text to the file from a read-only span using the default encoding.
    /// </summary>
    /// <remarks>Wraps the span-based <c>File.AppendAllText</c> overload.</remarks>
    /// <param name="contents">The text to append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendAllText(ReadOnlySpan<char> contents)
    {
        File.AppendAllText(FullName, contents);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Appends all text to the file from a read-only span using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps the span-based <c>File.AppendAllText</c> overload.</remarks>
    /// <param name="contents">The text to append.</param>
    /// <param name="encoding">The encoding to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendAllText(ReadOnlySpan<char> contents, Encoding encoding)
    {
        File.AppendAllText(FullName, contents, encoding);
    }
#endif

    /// <summary>
    /// Appends all lines to the file using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.AppendAllLines(string,System.Collections.Generic.IEnumerable{string})" />.</remarks>
    /// <param name="contents">The lines to append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendAllLines(IEnumerable<string> contents)
    {
        File.AppendAllLines(FullName, contents);
    }

    /// <summary>
    /// Appends all lines to the file using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.AppendAllLines(string,System.Collections.Generic.IEnumerable{string},System.Text.Encoding)" />.</remarks>
    /// <param name="contents">The lines to append.</param>
    /// <param name="encoding">The encoding to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendAllLines(IEnumerable<string> contents, Encoding encoding)
    {
        File.AppendAllLines(FullName, contents, encoding);
    }

#if NET9_0_OR_GREATER
    /// <summary>
    /// Appends all bytes to the file.
    /// </summary>
    /// <remarks>Wraps the byte-array <c>File.AppendAllBytes</c> overload.</remarks>
    /// <param name="bytes">The bytes to append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendAllBytes(byte[] bytes)
    {
        File.AppendAllBytes(FullName, bytes);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Appends all bytes to the file from a read-only span.
    /// </summary>
    /// <remarks>Wraps the span-based <c>File.AppendAllBytes</c> overload.</remarks>
    /// <param name="bytes">The bytes to append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendAllBytes(ReadOnlySpan<byte> bytes)
    {
        File.AppendAllBytes(FullName, bytes);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Reads all text from the file asynchronously using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllTextAsync(string,System.Threading.CancellationToken)" />.</remarks>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The file contents.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task<string> ReadAllTextAsync(CancellationToken cancellationToken = default)
    {
        return File.ReadAllTextAsync(FullName, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Reads all text from the file asynchronously using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllTextAsync(string,System.Text.Encoding,System.Threading.CancellationToken)" />.</remarks>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The file contents.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task<string> ReadAllTextAsync(Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.ReadAllTextAsync(FullName, encoding, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Reads all bytes from the file asynchronously.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllBytesAsync(string,System.Threading.CancellationToken)" />.</remarks>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The file bytes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task<byte[]> ReadAllBytesAsync(CancellationToken cancellationToken = default)
    {
        return File.ReadAllBytesAsync(FullName, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Reads all lines from the file asynchronously using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllLinesAsync(string,System.Threading.CancellationToken)" />.</remarks>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The file lines.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task<string[]> ReadAllLinesAsync(CancellationToken cancellationToken = default)
    {
        return File.ReadAllLinesAsync(FullName, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Reads all lines from the file asynchronously using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ReadAllLinesAsync(string,System.Text.Encoding,System.Threading.CancellationToken)" />.</remarks>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The file lines.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task<string[]> ReadAllLinesAsync(Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.ReadAllLinesAsync(FullName, encoding, cancellationToken);
    }
#endif

#if NET7_0_OR_GREATER
    /// <summary>
    /// Reads the file line by line asynchronously using the default encoding.
    /// </summary>
    /// <remarks>Wraps the asynchronous <c>File.ReadLinesAsync</c> overload.</remarks>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The file lines.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IAsyncEnumerable<string> ReadLinesAsync(CancellationToken cancellationToken = default)
    {
        return File.ReadLinesAsync(FullName, cancellationToken);
    }
#endif

#if NET7_0_OR_GREATER
    /// <summary>
    /// Reads the file line by line asynchronously using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps the asynchronous encoded <c>File.ReadLinesAsync</c> overload.</remarks>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The file lines.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IAsyncEnumerable<string> ReadLinesAsync(Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.ReadLinesAsync(FullName, encoding, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Writes all text to the file asynchronously using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.WriteAllTextAsync(string,string,System.Threading.CancellationToken)" />.</remarks>
    /// <param name="contents">The text to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the write operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task WriteAllTextAsync(string contents, CancellationToken cancellationToken = default)
    {
        return File.WriteAllTextAsync(FullName, contents, cancellationToken);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Writes all text to the file asynchronously from a read-only memory buffer using the default encoding.
    /// </summary>
    /// <remarks>Wraps the memory-based <c>File.WriteAllTextAsync</c> overload.</remarks>
    /// <param name="contents">The text to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the write operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task WriteAllTextAsync(ReadOnlyMemory<char> contents, CancellationToken cancellationToken = default)
    {
        return File.WriteAllTextAsync(FullName, contents, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Writes all text to the file asynchronously using the supplied encoding.
    /// </summary>
    /// <remarks>
    /// Wraps <see cref="File.WriteAllTextAsync(string,string,System.Text.Encoding,System.Threading.CancellationToken)" />.
    /// </remarks>
    /// <param name="contents">The text to write.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the write operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task WriteAllTextAsync(string contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.WriteAllTextAsync(FullName, contents, encoding, cancellationToken);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Writes all text to the file asynchronously from a read-only memory buffer using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps the memory-based encoded <c>File.WriteAllTextAsync</c> overload.</remarks>
    /// <param name="contents">The text to write.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the write operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task WriteAllTextAsync(ReadOnlyMemory<char> contents, Encoding encoding,
        CancellationToken cancellationToken = default)
    {
        return File.WriteAllTextAsync(FullName, contents, encoding, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Writes all bytes to the file asynchronously.
    /// </summary>
    /// <remarks>Wraps <see cref="File.WriteAllBytesAsync(string,byte[],System.Threading.CancellationToken)" />.</remarks>
    /// <param name="bytes">The bytes to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the write operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task WriteAllBytesAsync(byte[] bytes, CancellationToken cancellationToken = default)
    {
        return File.WriteAllBytesAsync(FullName, bytes, cancellationToken);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Writes all bytes to the file from a read-only span.
    /// </summary>
    /// <remarks>Wraps the span-based <c>File.WriteAllBytes</c> overload.</remarks>
    /// <param name="bytes">The bytes to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAllBytes(ReadOnlySpan<byte> bytes)
    {
        File.WriteAllBytes(FullName, bytes);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Writes all bytes to the file asynchronously from a read-only memory buffer.
    /// </summary>
    /// <remarks>Wraps the memory-based <c>File.WriteAllBytesAsync</c> overload.</remarks>
    /// <param name="bytes">The bytes to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the write operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task WriteAllBytesAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken = default)
    {
        return File.WriteAllBytesAsync(FullName, bytes, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Writes all lines to the file asynchronously using the default encoding.
    /// </summary>
    /// <remarks>
    /// Wraps <c>File.WriteAllLinesAsync(string, IEnumerable&lt;string&gt;, CancellationToken)</c>.
    /// </remarks>
    /// <param name="contents">The lines to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the write operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task WriteAllLinesAsync(IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        return File.WriteAllLinesAsync(FullName, contents, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Writes all lines to the file asynchronously using the supplied encoding.
    /// </summary>
    /// <remarks>
    /// Wraps <c>File.WriteAllLinesAsync(string, IEnumerable&lt;string&gt;, Encoding, CancellationToken)</c>.
    /// </remarks>
    /// <param name="contents">The lines to write.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the write operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task WriteAllLinesAsync(IEnumerable<string> contents, Encoding encoding,
        CancellationToken cancellationToken = default)
    {
        return File.WriteAllLinesAsync(FullName, contents, encoding, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Appends all text to the file asynchronously using the default encoding.
    /// </summary>
    /// <remarks>Wraps <see cref="File.AppendAllTextAsync(string,string,System.Threading.CancellationToken)" />.</remarks>
    /// <param name="contents">The text to append.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the append operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task AppendAllTextAsync(string contents, CancellationToken cancellationToken = default)
    {
        return File.AppendAllTextAsync(FullName, contents, cancellationToken);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Appends all text to the file asynchronously from a read-only memory buffer using the default encoding.
    /// </summary>
    /// <remarks>Wraps the memory-based <c>File.AppendAllTextAsync</c> overload.</remarks>
    /// <param name="contents">The text to append.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the append operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task AppendAllTextAsync(ReadOnlyMemory<char> contents, CancellationToken cancellationToken = default)
    {
        return File.AppendAllTextAsync(FullName, contents, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Appends all text to the file asynchronously using the supplied encoding.
    /// </summary>
    /// <remarks>
    /// Wraps <see cref="File.AppendAllTextAsync(string,string,System.Text.Encoding,System.Threading.CancellationToken)" />.
    /// </remarks>
    /// <param name="contents">The text to append.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the append operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task AppendAllTextAsync(string contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.AppendAllTextAsync(FullName, contents, encoding, cancellationToken);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Appends all text to the file asynchronously from a read-only memory buffer using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps the memory-based encoded <c>File.AppendAllTextAsync</c> overload.</remarks>
    /// <param name="contents">The text to append.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the append operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task AppendAllTextAsync(ReadOnlyMemory<char> contents, Encoding encoding,
        CancellationToken cancellationToken = default)
    {
        return File.AppendAllTextAsync(FullName, contents, encoding, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Appends all lines to the file asynchronously using the default encoding.
    /// </summary>
    /// <remarks>
    /// Wraps <c>File.AppendAllLinesAsync(string, IEnumerable&lt;string&gt;, CancellationToken)</c>.
    /// </remarks>
    /// <param name="contents">The lines to append.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the append operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task AppendAllLinesAsync(IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        return File.AppendAllLinesAsync(FullName, contents, cancellationToken);
    }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Appends all lines to the file asynchronously using the supplied encoding.
    /// </summary>
    /// <remarks>
    /// Wraps <c>File.AppendAllLinesAsync(string, IEnumerable&lt;string&gt;, Encoding, CancellationToken)</c>.
    /// </remarks>
    /// <param name="contents">The lines to append.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the append operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task AppendAllLinesAsync(IEnumerable<string> contents, Encoding encoding,
        CancellationToken cancellationToken = default)
    {
        return File.AppendAllLinesAsync(FullName, contents, encoding, cancellationToken);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Appends all bytes to the file asynchronously.
    /// </summary>
    /// <remarks>Wraps <see cref="File.AppendAllBytesAsync(string,byte[],System.Threading.CancellationToken)" />.</remarks>
    /// <param name="bytes">The bytes to append.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the append operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task AppendAllBytesAsync(byte[] bytes, CancellationToken cancellationToken = default)
    {
        return File.AppendAllBytesAsync(FullName, bytes, cancellationToken);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Appends all bytes to the file asynchronously from a read-only memory buffer.
    /// </summary>
    /// <remarks>Wraps the memory-based <c>File.AppendAllBytesAsync</c> overload.</remarks>
    /// <param name="bytes">The bytes to append.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the append operation finishes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task AppendAllBytesAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken = default)
    {
        return File.AppendAllBytesAsync(FullName, bytes, cancellationToken);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Writes all text to the file from a read-only span using the default encoding.
    /// </summary>
    /// <remarks>Wraps the span-based <c>File.WriteAllText</c> overload.</remarks>
    /// <param name="contents">The text to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAllText(ReadOnlySpan<char> contents)
    {
        File.WriteAllText(FullName, contents);
    }
#endif

#if NET9_0_OR_GREATER
    /// <summary>
    /// Writes all text to the file from a read-only span using the supplied encoding.
    /// </summary>
    /// <remarks>Wraps the span-based encoded <c>File.WriteAllText</c> overload.</remarks>
    /// <param name="contents">The text to write.</param>
    /// <param name="encoding">The encoding to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAllText(ReadOnlySpan<char> contents, Encoding encoding)
    {
        File.WriteAllText(FullName, contents, encoding);
    }
#endif
}