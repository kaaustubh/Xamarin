#include "xamarin/xamarin.h"

extern void *mono_aot_module_CameraExample_info;
extern void *mono_aot_module_monotouch_info;
extern void *mono_aot_module_mscorlib_info;

void xamarin_register_modules ()
{
	mono_aot_register_module (mono_aot_module_CameraExample_info);
	mono_aot_register_module (mono_aot_module_monotouch_info);
	mono_aot_register_module (mono_aot_module_mscorlib_info);

}

void xamarin_register_assemblies ()
{
	xamarin_open_and_register ("monotouch.dll");

}

void xamarin_setup ()
{
	xamarin_use_old_dynamic_registrar = FALSE;
	xamarin_create_classes ();
	xamarin_enable_debug_tracking = FALSE;
	xamarin_executable_name = "CameraExample.exe";
	xamarin_use_new_assemblies = 0;
	mono_use_llvm = FALSE;
	xamarin_log_level = 0;
	xamarin_debug_mode = TRUE;
	xamarin_new_refcount = FALSE;
}

int main (int argc, char **argv)
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	int rv = xamarin_main (argc, argv, false);
	[pool drain];
	return rv;
}
