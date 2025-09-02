using FluentAssertions;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;
using Serilog.Sinks.Graylog.Core.MessageBuilders;
using Serilog.Sinks.Graylog.Tests;
using System;
using System.Text.Json.Nodes;
using Xunit;

namespace Serilog.Sinks.Graylog.Core.Tests.MessageBuilders
{
    public class ExceptionMessageBuilderFixture
    {
        [Fact]
        public void WhenCreateException_ThenBuildShoulNotThrow()
        {
            var options = new GraylogSinkOptions();

            ExceptionMessageBuilder exceptionBuilder = new("localhost", options);

            Exception testExc = null;

            try
            {
                try
                {
                    throw new InvalidOperationException("Level One exception");
                } catch (Exception exc)
                {
                    throw new NotImplementedException("Nested Exception", exc);
                }
            } catch (Exception exc)
            {
                testExc = exc;
            }


            DateTimeOffset date = DateTimeOffset.Now;
            LogEvent logEvent = LogEventSource.GetExceptionLogEvent(date, testExc);

            JsonObject obj = exceptionBuilder.Build(logEvent);

            obj.Should().NotBeNull();
        }

        [Fact]
        public void WhenBuildWithTextException_AddsExceptionStringProperty()
        {
            // Arrange
            DateTimeOffset date = DateTimeOffset.Now;
            GraylogSinkOptions options = new GraylogSinkOptions();
            ExceptionMessageBuilder exceptionBuilder = new("localhost", options);
            Exception testException = null;
            string exceptionString = "Test exception message";

            try
            {
                throw new Serilog.Formatting.Compact.Reader.TextException(exceptionString);
            }
            catch (Exception ex)
            {
                testException = ex;
            }

            LogEvent logEvent = LogEventSource.GetExceptionLogEvent(date, testException);

            // Act
            JsonObject obj = exceptionBuilder.Build(logEvent);

            // Assert
            obj.Should().ContainKey("_ExceptionString").WhoseValue.ToString().Should().Be(exceptionString);
        }
    }
}
