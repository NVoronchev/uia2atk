#
# spec file for package UiaDbus
#

Name:           uiadbus
Version:        138999
Release:        1
License:        MIT
Group:          System/Libraries
URL:		http://www.mono-project.com/Accessibility
Source0:        %{name}-%{version}.tar.bz2
BuildRoot:      %{_tmppath}/%{name}-%{version}-build
Summary:        UiaDbus components of UIA on Linux
BuildRequires:	glib-sharp2
BuildRequires:	mono-devel
BuildRequires:	mono-uia-devel
BuildRequires:	ndesk-dbus

%description
UiaDbus is another communication channel for UIA on Linux between the client and provider

%package devel
License:        MIT
Summary:        Devel package for UiaDbus
Group:          Development/Libraries/Mono
Requires:       uiadbuscorebridge == %{version}-%{release}

%description devel
UiaDbus is another communication channel for UIA on Linux between the client and provider

%prep
%setup -q

%build
%configure --disable-tests
make %{?_smp_mflags}

%install
%makeinstall


%clean
rm -rf %{buildroot}

%files
%defattr(-,root,root)
%dir %{_prefix}/lib/mono/gac/UiaDbus
%{_prefix}/lib/mono/gac/UiaDbus/*
%dir %{_prefix}/lib/mono/gac/UiaDbusBridge
%{_prefix}/lib/mono/gac/UiaDbusBridge/*
%dir %{_prefix}/lib/mono/gac/UiaDbusSource
%{_prefix}/lib/mono/gac/UiaDbusSource/*
%{_prefix}/lib/mono/accessibility/UiaDbus.dll
%dir %{_libdir}/uiadbus
%{_libdir}/uiadbus/UiaDbus.dll*
%{_libdir}/uiadbus/UiaDbusBridge.dll*
%{_libdir}/uiadbus/UiaDbusSource.dll*

%files devel
%defattr(-,root,root)
%{_libdir}/pkgconfig/mono-uia-dbus.pc

%changelog