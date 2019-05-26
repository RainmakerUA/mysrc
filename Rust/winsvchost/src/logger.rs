extern crate chrono;

use std::fs::OpenOptions;
use std::io::prelude::*;
use chrono::prelude::*;

static LOG_FILE: &'static str = r#"E:\winsvchost.log"#;

pub fn log_str(s: &str) {
    let timestamp = Utc::now().format("%Y-%m-%d %H:%M:%S").to_string();
    match OpenOptions::new().create(true).write(true).append(true).open(LOG_FILE) {
        Ok(mut file) => if let Err(writeln_err) = writeln!(file, "{}: {}", timestamp, s) {
            eprintln!("Cannot write to file: {}", writeln_err);
        },
        Err(e) => eprintln!("Cannot open log file: {}", e)
    }
}
