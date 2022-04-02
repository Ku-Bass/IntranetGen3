﻿using MensaGymnazium.IntranetGen3.DataLayer.Repositories;
using MensaGymnazium.IntranetGen3.DataLayer.Repositories.Security;
using MensaGymnazium.IntranetGen3.DataLayer.Seeds.Core;
using MensaGymnazium.IntranetGen3.Model;
using MensaGymnazium.IntranetGen3.Primitives;

namespace MensaGymnazium.IntranetGen3.DataLayer.Seeds.Demo;

public class SubjectRegistrationSeed : DataSeed<DemoProfile>
{
	private readonly IUserRepository userRepository;
	private readonly ISigningRuleRepository signingRuleRepository;
	private readonly ISubjectRepository subjectRepository;

	public SubjectRegistrationSeed(
		IUserRepository userRepository,
		ISigningRuleRepository signingRuleRepository,
		ISubjectRepository subjectRepository)
	{
		this.userRepository = userRepository;
		this.signingRuleRepository = signingRuleRepository;
		this.subjectRepository = subjectRepository;
	}

	public override void SeedData()
	{
		var users = userRepository.GetAll();
		var signingRules = signingRuleRepository.GetAll();
		var subjects = subjectRepository.GetAll();

		var registrations = new StudentSubjectRegistration[]
		{
			new StudentSubjectRegistration()
			{
				UsedSigningRuleId = signingRules.First(sr => sr.Name.Equals("Sekunda - Specializovaný seminář nebo jazyk")).Id,
				StudentId = users.First(s => s.Email.StartsWith("sara.sekundova")).StudentId.Value,
				SubjectId = subjects.First(s => s.Name.Equals("Brdek je bůh")).Id,
				RegistrationType = StudentRegistrationType.Main
			},
			new StudentSubjectRegistration()
			{
				UsedSigningRuleId = signingRules.First(sr => sr.Name.Equals("Tercie - Specializovaný seminář")).Id,
				StudentId = users.First(s => s.Email.StartsWith("tereza.tercianova")).StudentId.Value,
				SubjectId = subjects.First(s => s.Name.Equals("Brdek je bůh")).Id,
				RegistrationType = StudentRegistrationType.Main
			}
			// TODO Více registrací
		};

		Seed(For(registrations).PairBy(r => r.StudentId, r => r.SubjectId, r => r.UsedSigningRuleId));
	}

	public override IEnumerable<Type> GetPrerequisiteDataSeeds()
	{
		yield return typeof(StudentSeed);
		yield return typeof(SubjectSeed);
	}
}
