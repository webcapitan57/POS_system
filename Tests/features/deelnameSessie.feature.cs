﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.7.0.0
//      SpecFlow Generator Version:3.7.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Tests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.7.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class DeelnemenSessieFeature : object, Xunit.IClassFixture<DeelnemenSessieFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "deelnameSessie.feature"
#line hidden
        
        public DeelnemenSessieFeature(DeelnemenSessieFeature.FixtureData fixtureData, Tests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "features", "deelnemen sessie", "Als leerling wil ik deelnemen aan een sessie zodat ik gebruik kan maken van de fo" +
                    "to app.", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
    #line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Question",
                        "IsRequired"});
            table1.AddRow(new string[] {
                        "1",
                        "what is your gender",
                        "false"});
            table1.AddRow(new string[] {
                        "2",
                        "how old are you",
                        "true"});
#line 6
  testRunner.Given("StudentProfileQuestions:", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "NeededStudentProfileQuestions"});
            table2.AddRow(new string[] {
                        "1",
                        "1,2"});
#line 11
        testRunner.Given("Setup:", ((string)(null)), table2, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "SetUp"});
            table3.AddRow(new string[] {
                        "1",
                        "1"});
#line 15
  testRunner.Given("Teacher:", ((string)(null)), table3, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Title"});
            table4.AddRow(new string[] {
                        "1",
                        "mijn buurt"});
            table4.AddRow(new string[] {
                        "2",
                        "onze buurt"});
#line 19
        testRunner.Given("Tasks:", ((string)(null)), table4, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Tasks",
                        "Teacher"});
            table5.AddRow(new string[] {
                        "1A1B1C",
                        "",
                        "1"});
            table5.AddRow(new string[] {
                        "2D2E2F",
                        "1",
                        "1"});
            table5.AddRow(new string[] {
                        "3G3H3I",
                        "1,2",
                        "1"});
#line 24
        testRunner.Given("Groups:", ((string)(null)), table5, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "Question",
                        "Answer"});
            table6.AddRow(new string[] {
                        "1",
                        "1",
                        "male"});
            table6.AddRow(new string[] {
                        "2",
                        "2",
                        "64"});
            table6.AddRow(new string[] {
                        "3",
                        "1",
                        ""});
            table6.AddRow(new string[] {
                        "4",
                        "2",
                        ""});
            table6.AddRow(new string[] {
                        "5",
                        "1",
                        ""});
            table6.AddRow(new string[] {
                        "6",
                        "2",
                        ""});
#line 30
    testRunner.Given("StudentProfileAnswers:", ((string)(null)), table6, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Id",
                        "ProfileAnswers",
                        "Group"});
            table7.AddRow(new string[] {
                        "1",
                        "1,2",
                        "2D2E2F"});
            table7.AddRow(new string[] {
                        "2",
                        "3,4",
                        "2D2E2F"});
            table7.AddRow(new string[] {
                        "3",
                        "5,6",
                        "2D2E2F"});
#line 39
        testRunner.Given("Students:", ((string)(null)), table7, "Given ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="A new student tries to join a group")]
        [Xunit.TraitAttribute("FeatureTitle", "deelnemen sessie")]
        [Xunit.TraitAttribute("Description", "A new student tries to join a group")]
        public virtual void ANewStudentTriesToJoinAGroup()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A new student tries to join a group", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 47
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
    this.FeatureBackground();
#line hidden
#line 48
        testRunner.When("A user inputs \'1A1B1C\' in the group code field", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 49
        testRunner.Then("student \'4\' is created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 50
        testRunner.And("student \'4\' is linked to group \'1A1B1C\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 51
  testRunner.And("StudentProfileAnswer \'7\' is created and linked to Student \'4\' and studentProfileQ" +
                        "uestion \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 52
  testRunner.And("StudentProfileAnswer \'8\' is created and linked to Student \'4\' and studentProfileQ" +
                        "uestion \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="A new student tries to join an unexisting group")]
        [Xunit.TraitAttribute("FeatureTitle", "deelnemen sessie")]
        [Xunit.TraitAttribute("Description", "A new student tries to join an unexisting group")]
        public virtual void ANewStudentTriesToJoinAnUnexistingGroup()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A new student tries to join an unexisting group", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 54
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
    this.FeatureBackground();
#line hidden
#line 55
        testRunner.When("A user inputs \'AAA999\' in the group code field", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 56
        testRunner.Then("There is no student \'4\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="A student answers all questions")]
        [Xunit.TraitAttribute("FeatureTitle", "deelnemen sessie")]
        [Xunit.TraitAttribute("Description", "A student answers all questions")]
        public virtual void AStudentAnswersAllQuestions()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A student answers all questions", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 58
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
    this.FeatureBackground();
#line hidden
#line 59
        testRunner.When("Student \'2\' gives studentProfileAnswer \'3\' value \'female\' to StudentProfileQuesti" +
                        "on \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 60
        testRunner.And("Student \'2\' gives studentProfileAnswer \'4\' value \'15\' to StudentProfileQuestion \'" +
                        "2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 61
        testRunner.Then("Value of studentProfileAnswer \'3\' is \'female\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 62
        testRunner.And("Value of studentProfileAnswer \'4\' is \'15\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 63
        testRunner.And("Delivery \'1\' is created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 64
        testRunner.And("Delivery \'1\' is linked to group \'2D2E2F\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="A student answers all mandatory questions")]
        [Xunit.TraitAttribute("FeatureTitle", "deelnemen sessie")]
        [Xunit.TraitAttribute("Description", "A student answers all mandatory questions")]
        public virtual void AStudentAnswersAllMandatoryQuestions()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A student answers all mandatory questions", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 66
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
    this.FeatureBackground();
#line hidden
#line 67
        testRunner.When("Student \'3\' gives studentProfileAnswer \'5\' value \'female\' to StudentProfileQuesti" +
                        "on \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 68
        testRunner.And("Student \'3\' gives studentProfileAnswer \'6\' no value to StudentProfileQuestion \'2\'" +
                        "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 69
        testRunner.Then("Value of studentProfileAnswer \'5\' is \'female\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 70
        testRunner.And("Value of studentProfileAnswer \'6\' is null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 71
        testRunner.And("Delivery \'1\' is created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 72
        testRunner.And("Delivery \'1\' is linked to group \'2D2E2F\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="A student answers no questions")]
        [Xunit.TraitAttribute("FeatureTitle", "deelnemen sessie")]
        [Xunit.TraitAttribute("Description", "A student answers no questions")]
        public virtual void AStudentAnswersNoQuestions()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A student answers no questions", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 74
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
    this.FeatureBackground();
#line hidden
#line 75
        testRunner.When("student \'2\' leaves all question fields blank and presses next", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 76
        testRunner.Then("no deliveries are created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.7.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                DeelnemenSessieFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                DeelnemenSessieFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
