import { ILogger, LogLevel } from "./ILogger";
export declare class NullLogger implements ILogger {
    log(logLevel: LogLevel, message: string): void;
}
export declare class ConsoleLogger implements ILogger {
    private readonly minimumLogLevel;
    constructor(minimumLogLevel: LogLevel);
    log(logLevel: LogLevel, message: string): void;
}
export declare namespace LoggerFactory {
    function createLogger(logging?: ILogger | LogLevel): ILogger;
}
