#![cfg(windows)]

#[macro_use]
extern crate windows_service;

mod logger;

use std::env;
//use std::fmt::Debug;
use std::ffi::OsString;
use windows_service::service_dispatcher;

static SVC_NAME: &'static str = "rm.winsvchost";

fn main() -> windows_service::Result<()> {
    logger::log_str(&format!("Service {} is contained here: 0x{:08X}", SVC_NAME, wrap_ref_ext(ffi_service_main)));
    service_dispatcher::start(SVC_NAME, ffi_service_main)?;
    logger::log_str("Service was started");
    Ok(())
}

define_windows_service!(ffi_service_main, service_main);

fn service_main(args: Vec<OsString>) {
    log_vec("Service Main():", &args.iter().map(|os_string| os_string.to_string_lossy().into_owned()).collect());
    log_vec("env::args():", &env::args().collect());
}

fn log_vec(title: &str, vec: &Vec<String>) {
    logger::log_str(title);
    for arg in vec {
        logger::log_str(arg);
    }
    logger::log_str("========================");
}

/*
fn wrap_ref<T1, T2, TRes, F: FnOnce(T1, T2) -> TRes>(f: &F) -> usize {
    (f as *const F ) as usize
}
*/

fn wrap_ref_ext(f: extern "system" fn(u32, *mut *mut u16)) -> usize {
    (f as *const u8 ) as usize
}
