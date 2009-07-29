#
# spec file for package at-spi-sharp
#

Name:           at-spi-sharp
Version:        1.9.0
Release:        1
License:        MIT
Group:          System/Libraries
URL:		http://www.mono-project.com/Accessibility
Source0:        %{name}-%{version}.tar.bz2
BuildRoot:      %{_tmppath}/%{name}-%{version}-build

Summary:        c# bindings for at-spi

%description
C# bindings for at-spi

%prep
%setup -q

%build
%configure
make %{?_smp_mflags}

%install
make DESTDIR=%{buildroot} install


%clean
rm -rf %{buildroot}

%files
%defattr(-,root,root)


%post

%postun

%changelog