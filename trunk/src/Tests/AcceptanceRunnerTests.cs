using ConeFabric.FairyTales.Acceptance;
using NMock2;
using NUnit.Framework;
using WatiN.Core.Interfaces;

namespace ConeFabric.FairyTales.Tests
{
    [TestFixture]
    public class AcceptanceRunnerTests
    {
        [Test]
        public void ArgumentsShouldBeAbleToIncludeWhiteSpace()
        {
            var input = new[] {"enter ProjectInput \"Fairy Tales\""};
            var runner = new AcceptanceRunner(input);
            Assert.AreEqual("\"Fairy Tales\"", runner.Commands[0].Argument);
        }

        [Test]
        public void CloseShouldAlwaysBeDone()
        {
            var input = new[] {"open ProjectPage"};
            var browser = new FakeBrowser();

            var runner = new AcceptanceRunner(input);
            runner.Run(browser);

            Assert.IsTrue(browser.DisposeCalled);
            Assert.IsTrue(runner.Results["open ProjectPage"]);
            Assert.IsTrue(runner.Results["close"]);
        }

        [Test]
        public void LocalHost666IsSettable()
        {
            var runner = new AcceptanceRunner(new string[] {}) {FairyTalesUrl = "http://localhost/fairytales/"};
            Assert.AreEqual("http://localhost/fairytales/", runner.FairyTalesUrl);
        }

        [Test]
        public void LocalhostAtPort666IsSetPerDefault()
        {
            var runner = new AcceptanceRunner(new string[] {});
            Assert.AreEqual("http://localhost:666/", runner.FairyTalesUrl);
        }

        [Test]
        public void RunnerCanAddCommands()
        {
            var input = new[] {"open ProjectPage"};
            var runner = new AcceptanceRunner(input);
            var command = runner.Commands[0];

            Assert.AreEqual("open", command.Action);
            Assert.AreEqual("ProjectPage", command.Subject);
        }

        [Test]
        public void RunnerCanEnterProjectName()
        {
            var input = new[] {"enter ProjectInput \"Fairy Tales\""};

            using (var mock = new Mockery())
            {
                var browser = mock.NewMock<IBrowser>();

                var projectInput = mock.NewMock<ITextField>();

                Expect.AtLeastOnce.On(browser).Method("TextField").With("ctl00_ContentPlaceHolder1_projectInput").Will(
                    Return.Value(projectInput));
                Expect.Once.On(projectInput).GetProperty("Exists").Will(Return.Value(true));
                Expect.Once.On(projectInput).SetProperty("Value").To("Fairy Tales");
                Expect.Once.On(projectInput).GetProperty("Value").Will(Return.Value("Fairy Tales"));
                Expect.Once.On(browser).Method("Dispose");

                var runner = new AcceptanceRunner(input);
                runner.Run(browser);

                Assert.AreEqual(true, runner.Results["enter ProjectInput \"Fairy Tales\""]);
            }
        }

        [Test]
        public void RunnerCanFindActiveProject()
        {
            var input = new[] {"check ActiveProject \"kek\""};

            var span = new FakeSpan {_text = "kek"};
            var browser = FakeBrowser.WithSpan(span);

            var runner = new AcceptanceRunner(input);
            runner.Run(browser);

            Assert.IsTrue(runner.Results["check ActiveProject \"kek\""]);
            Assert.IsTrue(span.getTextCalled);
        }

        [Test]
        public void RunnerCanPressAddProject()
        {
            var input = new[] {"press AddProject"};
            var mock = new Mockery();


            var button = new FakeButton();
            var browser = FakeBrowser.WithButton(button);

            var runner = new AcceptanceRunner(input);
            runner.Run(browser);

            Assert.IsTrue(runner.Results["press AddProject"]);
            Assert.IsTrue(button.clickCalled);

            mock.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void RunnerShouldBeAbleToHaveSeveralCommands()
        {
            var input = new[] {"open ProjectPage", "check ActiveProject \"\""};
            var runner = new AcceptanceRunner(input);
            var commands = runner.Commands;
            Assert.AreEqual("open", commands[0].Action);
            Assert.AreEqual("ProjectPage", commands[0].Subject);
            Assert.AreEqual("check", commands[1].Action);
            Assert.AreEqual("ActiveProject", commands[1].Subject);
            Assert.AreEqual("\"\"", commands[1].Argument);
        }

        [Test]
        public void RunnerShouldUseWatinBrowserToManageTests()
        {
            var input = new[] {"open ProjectPage"};
            using (var mock = new Mockery())
            {
                var browser = mock.NewMock<IBrowser>();
                Expect.Once.On(browser).Method("GoTo").With(new[] {"http://localhost:666/ProjectPage.aspx"});
                Expect.Once.On(browser).GetProperty("Url").Will(Return.Value("http://localhost:666/ProjectPage.aspx"));
                Expect.Once.On(browser).Method("Dispose");

                var runner = new AcceptanceRunner(input);
                runner.Run(browser);
            }
        }
    }
}