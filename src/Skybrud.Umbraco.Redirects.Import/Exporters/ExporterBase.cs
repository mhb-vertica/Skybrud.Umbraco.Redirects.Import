﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Models;
using System;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Exporters;

/// <summary>
/// Class serving as a base implementation for an <see cref="IExporter"/>.
/// </summary>
/// <typeparam name="TOptions">The type of the options.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public abstract class ExporterBase<TOptions, TResult> : IExporter<TOptions, TResult> where TOptions : IExportOptions where TResult : IExportResult {

    #region Properties

    /// <summary>
    /// Gets the type of the importer.
    /// </summary>
    [JsonProperty("type", Order = -99)]
    public string Type => GetType().AssemblyQualifiedName!;

    /// <summary>
    /// Gets the icon of the importer.
    /// </summary>
    [JsonProperty("icon", Order = -98)]
    public string Icon { get; protected set; } = "icon-binarycode";

    /// <summary>
    /// Gets the name of the importer.
    /// </summary>
    [JsonProperty("name", Order = -97)]
    public string Name { get; protected set; }

    /// <summary>
    /// Gets the description of the importer.
    /// </summary>
    [JsonProperty("description", Order = -96)]
    public string? Description { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance with default options.
    /// </summary>
    protected ExporterBase() {
        Name = GetType().Name;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Parses and returns the options based on the specified HTTP <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>A collection of <see cref="Option"/> representing the parsed options.</returns>
    public virtual IEnumerable<Option> GetOptions(HttpRequest request) {
        return Array.Empty<Option>();
    }

    IExportOptions IExporter.ParseOptions(JObject config) {
        return ParseOptions(config);
    }

    /// <summary>
    /// Parses the specified <paramref name="config"/> into an instance of <typeparamref name="TOptions"/>.
    /// </summary>
    /// <param name="config">A JSON object representing the configuration to parse.</param>
    /// <returns>An instance of <typeparamref name="TOptions"/> representing the parsed options.</returns>
    public virtual TOptions ParseOptions(JObject config) {
        return config.ToObject<TOptions>()!;
    }

    IExportResult IExporter.Export(IExportOptions options) {
        if (options is not TOptions t) throw new ArgumentException($"Must be an instance of '{typeof(TOptions)}'", nameof(options));
        return Export(t);
    }

    /// <summary>
    /// Triggers a new export based on the specified <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The options for the export.</param>
    /// <returns>An instance of <typeparamref name="TResult"/> representing the result of the export.</returns>
    public abstract TResult Export(TOptions options);

    #endregion

}