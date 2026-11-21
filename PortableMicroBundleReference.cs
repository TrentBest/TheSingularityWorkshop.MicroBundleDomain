using System.Text;

namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Identifies one immutable MicroBundle revision using its portable semantic identity.
/// </summary>
/// <remarks>
/// This value deliberately contains no repository storage handle or platform-local runtime ID.
/// The digest is optional only when the upstream immutable-artifact contract has not supplied one;
/// a consumer requiring byte-level pinning must reject a reference without it.
/// </remarks>
public readonly record struct PortableMicroBundleReference
{
    public PortableMicroBundleReference(string ontology, string version, string? sha256 = null)
    {
        if (!IsCanonicalOntology(ontology))
            throw new ArgumentException("A canonical, non-empty portable ontology identifier is required.", nameof(ontology));

        if (!IsPortableVersion(version))
            throw new ArgumentException("A non-empty portable exact artifact version is required.", nameof(version));

        if (sha256 is not null && !IsSha256(sha256))
            throw new ArgumentException("SHA-256 must contain exactly 64 hexadecimal characters.", nameof(sha256));

        Ontology = ontology;
        Version = version;
        Sha256 = sha256?.ToLowerInvariant();
    }

    /// <summary>Gets the canonical portable ontology identifier.</summary>
    public string Ontology { get; }

    /// <summary>Gets the exact immutable artifact version.</summary>
    public string Version { get; }

    /// <summary>Gets the optional SHA-256 digest of the exact artifact bytes.</summary>
    public string? Sha256 { get; }

    /// <summary>Gets whether this value is fully initialized and valid.</summary>
    public bool IsValid =>
        IsCanonicalOntology(Ontology) &&
        IsPortableVersion(Version) &&
        (Sha256 is null || IsSha256(Sha256));

    /// <summary>Gets whether this reference pins the exact artifact bytes.</summary>
    public bool IsContentPinned => IsValid && Sha256 is not null;

    private static bool IsCanonicalOntology(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) ||
            !string.Equals(value, value.Trim(), StringComparison.Ordinal) ||
            !string.Equals(value, value.Normalize(NormalizationForm.FormC), StringComparison.Ordinal))
            return false;

        if (value.Any(character => char.IsWhiteSpace(character) || char.IsControl(character) ||
                                   character is '?' or '#' or '%' or '\\'))
            return false;

        var segments = value.Split('/');
        return segments.All(segment =>
            segment.Length > 0 &&
            segment is not "." and not "..");
    }

    private static bool IsPortableVersion(string? value) =>
        !string.IsNullOrWhiteSpace(value) &&
        string.Equals(value, value.Trim(), StringComparison.Ordinal) &&
        !value.Any(character => char.IsWhiteSpace(character) || char.IsControl(character) ||
                                character is '/' or '\\' or '?' or '#' or '%');

    private static bool IsSha256(string value) =>
        value.Length == 64 && value.All(Uri.IsHexDigit);
}
