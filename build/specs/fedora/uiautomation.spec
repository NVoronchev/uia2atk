#
# spec file for package UIAutomation
#

Name:           mono-uia
Version:        1.0
Release:        1
License:        MIT/X11
Group:          System/Libraries
URL:		http://www.mono-project.com/Accessibility
Source0:        %{name}-%{version}.tar.bz2
BuildRoot:	%{_tmppath}/%{name}-%{version}-build
Requires:	mono-core >= 2.4
BuildRequires:	mono-core >= 2.4 mono-devel >= 2.4 mono-nunit >= 2.4
Summary:        Implementations of members and interfaces based on MS UIA API

%description
Implementations of the members and interfaces based on MS UIA API

%prep
%setup -q

%build
%configure
make

%install
make DESTDIR=%{buildroot} install

%clean
rm -rf %{buildroot}

%files
%defattr(-,root,root)
%doc README COPYING NEWS
%{_libdir}/mono/accessibility
%{_libdir}/mono/gac/UIAutomationProvider
%{_libdir}/mono/accessibility/UIAutomationProvider.dll
%{_libdir}/mono/gac/UIAutomationTypes
%{_libdir}/mono/accessibility/UIAutomationTypes.dll
%{_libdir}/mono/gac/UIAutomationBridge
%{_libdir}/mono/accessibility/UIAutomationBridge.dll
%{_libdir}/mono/gac/UIAutomationClient
%{_libdir}/mono/accessibility/UIAutomationClient.dll
%{_libdir}/pkgconfig/*.pc

%package -n mono-winfxcore
License:	MIT/X11
Summary:	Parts of winfx
Group:		Development/Languages/Mono
Requires:	mono-core >= 2.4

%description -n mono-winfxcore
The Mono Project is an open development initiative that is working to
develop an open source, Unix version of the .NET development platform.
Its objective is to enable Unix developers to build and deploy
cross-platform .NET applications. The project will implement various
technologies that have been submitted to the ECMA for standardization.

Parts of winfx

%files -n mono-winfxcore
%defattr(-, root, root)
%{_libdir}/mono/gac/WindowsBase
%{_libdir}/mono/2.0/WindowsBase.dll

%changelog
* Thu Apr 30 2009 Stephen Shaw <sshaw@decriptor.com> - 1.0
- Initial RPM