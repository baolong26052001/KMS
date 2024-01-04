using Insurance.ViewModel;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;
using static Insurance.View.HealthPackageView;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Linq;

namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for HealthPackageView.xaml
    /// </summary>
    public partial class HealthPackageView : UserControl
    {
        public HealthPackageView()
        {
            InitializeComponent();

            FetchInsurancePackages();
            FetchPackageDetails(1);

        }

        public class InsurancePackage
        {
            public int id { get; set; }
            public string packageName { get; set; }
            public int insuranceType { get; set; }
            public string typeName { get; set; }
            public int duration { get; set; }
            public string payType { get; set; }
            public int fee { get; set; }
            public DateTime dateModified { get; set; }
            public DateTime dateCreated { get; set; }
        }

        public class InsurancePackageDetail
        {
            public int id { get; set; }
            public string content { get; set; }
            public int coverage { get; set; }
            public string description { get; set; }
            public int packageId { get; set; }
            public DateTime dateModified { get; set; }
            public DateTime dateCreated { get; set; }
        }

        public class BenefitDetail
        {
            public int id { get; set; }
            public string content { get; set; }
            public int coverage { get; set; }
            public int benefitId { get; set; }
            public DateTime dateModified { get; set; }
            public DateTime dateCreated { get; set; }
        }

        private async void FetchInsurancePackages()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7017/api/InsurancePackage/ShowInsurancePackage";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        List<InsurancePackage> insurancePackages = JsonConvert.DeserializeObject<List<InsurancePackage>>(jsonResponse);

                        UpdateUI(insurancePackages);
                    }
                    else
                    {
                        // Handle unsuccessful API request
                        // You might want to log or display an error message
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You might want to log or display an error message
            }
        }

        private void UpdateUI(List<InsurancePackage> insurancePackages)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");

            // Assuming StackPanel is the name of your StackPanel in XAML
            StackPanel stackPanel = FindName("StackPanel") as StackPanel;

            if (stackPanel != null)
            {
                for (int i = 0; i < insurancePackages.Count; i++)
                {
                    Button packageButton = new Button
                    {
                        Content = $"Gói {i + 1}: {insurancePackages[i].fee.ToString("C", cultureInfo)}/năm",
                        Style = i == 0 ? (Style)FindResource("PackageButtonSelected") : (Style)FindResource("PackageButton"),
                        Width = 367,
                        Margin = new Thickness(0, 0, 0, 14),
                        Tag = insurancePackages[i].id // Set the Tag property to store the insurancePackage.id
                    };

                    // Add a click event handler for the button
                    packageButton.Click += PackageButton_Click;

                    stackPanel.Children.Add(packageButton);
                }
            }
        }

        private async void FetchPackageDetails(int packageId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://localhost:7017/api/InsurancePackage/ShowInsurancePackageDetail/{packageId}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        List<InsurancePackageDetail> packageDetail = JsonConvert.DeserializeObject<List<InsurancePackageDetail>>(jsonResponse);

                        if (packageDetail.Count > 0)
                        {
                            // Display the details if data is available
                            UpdateBenefitsUI(packageDetail);
                        }
                        else
                        {
                            // Clear the UI if no data is available
                            ClearBenefitsUI();
                        }
                    }
                    else
                    {
                        // Handle unsuccessful API request
                        // You might want to log or display an error message
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You might want to log or display an error message
            }
        }

        private async void UpdateBenefitsUI(List<InsurancePackageDetail> packageDetails)
        {
            StackPanel stackPanelBenefit = FindName("stackPanelBenefit") as StackPanel;
            CultureInfo cultureInfo = new CultureInfo("vi-VN");

            if (stackPanelBenefit != null)
            {
                stackPanelBenefit.Children.Clear();

                foreach (var packageDetail in packageDetails)
                {
                    StackPanel benefitPanel = new StackPanel { Margin = new Thickness(5) };

                    DockPanel dockPanel = new DockPanel();
                    TextBlock benefitText = new TextBlock
                    {
                        Text = $"{packageDetail.content}",
                        Foreground = Brushes.Black,
                        FontFamily = new FontFamily("Exo"),
                        FontWeight = FontWeights.Bold,
                        FontSize = 16
                    };

                    TextBlock coverageText = new TextBlock
                    {
                        Text = $"{packageDetail.coverage.ToString("C", cultureInfo)}",
                        Foreground = Brushes.Black,
                        FontWeight = FontWeights.Bold,
                        FontSize = 16,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };

                    TextBlock descriptionText = new TextBlock
                    {
                        Text = $"{packageDetail.description}",
                        Foreground = Brushes.Black,
                        FontFamily = new FontFamily("Exo"),
                        FontSize = 16
                    };

                    dockPanel.Children.Add(benefitText);
                    dockPanel.Children.Add(coverageText);
                    
                    benefitPanel.Children.Add(dockPanel);
                    benefitPanel.Children.Add(descriptionText);
                    // Fetch and display benefit details
                    await FetchAndDisplayBenefitDetails(packageDetail.id, benefitPanel);

                    stackPanelBenefit.Children.Add(benefitPanel);
                }
            }
        }

        private async Task FetchAndDisplayBenefitDetails(int packageDetailId, StackPanel benefitPanel)
        {
            // Fetch BenefitDetails
            List<BenefitDetail> benefitDetails = await FetchBenefitDetails(packageDetailId);

            // Display the BenefitDetails
            UpdateBenefitDetailsUI(benefitDetails, benefitPanel);
        }

        private void UpdateBenefitDetailsUI(List<BenefitDetail> benefitDetails, StackPanel benefitPanel)
        {
            // Add a nested stack panel for displaying BenefitDetails
            StackPanel nestedStackPanel = new StackPanel { Margin = new Thickness(10, 0, 0, 0) };

            // Assuming gridBenefitDetails is the name of your Grid in XAML
            Grid gridBenefitDetails = FindName("gridBenefitDetails") as Grid;
            CultureInfo cultureInfo = new CultureInfo("vi-VN");

            if (gridBenefitDetails != null)
            {
                // Clear existing children in the grid
                gridBenefitDetails.Children.Clear();

                if (benefitDetails.Any())
                {
                    foreach (var benefitDetail in benefitDetails)
                    {
                        // Create a new TextBlock for displaying benefit details
                        TextBlock benefitDetailText = new TextBlock
                        {
                            Margin = new Thickness(10, 0, 0, 0),
                            TextWrapping = TextWrapping.Wrap,
                            Width = 300,
                            Text = $"{benefitDetail.content}: {benefitDetail.coverage.ToString("C", cultureInfo)}",
                            Foreground = Brushes.Black,
                            Padding = new Thickness(5),
                            HorizontalAlignment = HorizontalAlignment.Left
                        };

                        // Add the benefit detail TextBlock to the nested stack panel
                        nestedStackPanel.Children.Add(benefitDetailText);
                    }

                    // Add the nested stack panel to the parent benefit panel
                    benefitPanel.Children.Add(nestedStackPanel);

                    // Show a success message
                    // MessageBox.Show("Benefit details fetched successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Show a message indicating no data
                    // MessageBox.Show("No benefit details available for the selected InsurancePackageDetail.", "No Data", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private async Task<List<BenefitDetail>> FetchBenefitDetails(int packageDetailId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://localhost:7017/api/InsurancePackage/ShowBenefit/{packageDetailId}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<BenefitDetail>>(jsonResponse);
                    }
                    else
                    {
                        // Handle unsuccessful response
                        return new List<BenefitDetail>();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return new List<BenefitDetail>();
            }
        }


        private void PackageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is int packageId)
            {
                ResetButtonStyles();
                clickedButton.Style = (Style)FindResource("PackageButtonSelected");

                // Fetch and display details for the selected InsurancePackage
                FetchPackageDetails(packageId);
            }
        }

        private void ResetButtonStyles()
        {
            StackPanel stackPanel = FindName("StackPanel") as StackPanel;

            if (stackPanel != null)
            {
                foreach (var child in stackPanel.Children)
                {
                    if (child is Button button)
                    {
                        button.Style = (Style)FindResource("PackageButton");
                    }
                }

                // Clear the UI when resetting button styles
                ClearBenefitsUI();
            }
        }

        private void ClearBenefitsUI()
        {
            // Assuming stackPanelBenefit is the name of your StackPanel in XAML
            StackPanel stackPanelBenefit = FindName("stackPanelBenefit") as StackPanel;

            if (stackPanelBenefit != null)
            {
                stackPanelBenefit.Children.Clear(); // Clear existing children

                // Optionally, you can display a message or perform other actions
                // For example:
                TextBlock noDataMessage = new TextBlock
                {
                    Text = "No data available for this package.",
                    Foreground = Brushes.Red,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                stackPanelBenefit.Children.Add(noDataMessage);
            }
        }
    }
}
