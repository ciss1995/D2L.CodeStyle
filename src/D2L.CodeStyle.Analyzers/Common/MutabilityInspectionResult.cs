﻿using Microsoft.CodeAnalysis;

namespace D2L.CodeStyle.Analyzers.Common {
	public enum MutabilityTarget {
		Member,
		Type,
		TypeArgument
	}

	public enum MutabilityCause {
		IsADelegate,
		IsNotReadonly,
		IsNotSealed,
		IsAnInterface,
		IsAnExternalUnmarkedType,
		IsAnArray,
		IsPotentiallyMutable,
		IsDynamic
	}

	public sealed class MutabilityInspectionResult {

		private readonly static MutabilityInspectionResult s_notMutableResult = new MutabilityInspectionResult( false, null, null, null, null );

		public bool IsMutable { get; }

		public string MemberPath { get; }

		public string TypeName { get; }

		public MutabilityCause? Cause { get; }

		public MutabilityTarget? Target { get; }

		private MutabilityInspectionResult(
			bool isMutable,
			string memberPath,
			string typeName,
			MutabilityTarget? target,
			MutabilityCause? cause
		) {
			IsMutable = isMutable;
			MemberPath = memberPath;
			TypeName = typeName;
			Target = target;
			Cause = cause;
		}

		public static MutabilityInspectionResult NotMutable() {
			return s_notMutableResult;
		}

		public static MutabilityInspectionResult Mutable(
			string mutableMemberPath,
			string membersTypeName,
			MutabilityTarget kind,
			MutabilityCause cause
		) {
			return new MutabilityInspectionResult(
				true,
				mutableMemberPath,
				membersTypeName,
				kind,
				cause
			);
		}

		public static MutabilityInspectionResult MutableType(
			ITypeSymbol type,
			MutabilityCause cause
		) {
			return Mutable(
				null,
				type.GetFullTypeName(),
				MutabilityTarget.Type,
				cause
			);
		}

		public static MutabilityInspectionResult MutableField(
			IFieldSymbol field,
			MutabilityCause cause
		) {
			return Mutable(
				field.Name,
				field.Type.GetFullTypeName(),
				MutabilityTarget.Member,
				cause
			);
		}

		public static MutabilityInspectionResult MutableProperty(
			IPropertySymbol property,
			MutabilityCause cause
		) {
			return Mutable(
				property.Name,
				property.Type.GetFullTypeName(),
				MutabilityTarget.Member,
				cause
			);
		}

		public static MutabilityInspectionResult PotentiallyMutableMember(
			ISymbol member
		) {
			return Mutable(
				member.Name,
				null,
				MutabilityTarget.Member,
				MutabilityCause.IsPotentiallyMutable
			);
		}

		public MutabilityInspectionResult WithPrefixedMember( string parentMember ) {
			var newMember = string.IsNullOrWhiteSpace( this.MemberPath )
				? parentMember
				: $"{parentMember}.{this.MemberPath}";

			return new MutabilityInspectionResult(
				this.IsMutable,
				newMember,
				this.TypeName,
				this.Target,
				this.Cause
			);
		}

		public MutabilityInspectionResult WithTarget( MutabilityTarget newTarget ) {
			return new MutabilityInspectionResult(
				this.IsMutable,
				this.MemberPath,
				this.TypeName,
				newTarget,
				this.Cause
			);
		}

	}
}
